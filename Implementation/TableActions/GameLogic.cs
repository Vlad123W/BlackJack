using BlackJack.Implementation.Data;
using BlackJack.Implementation.Entities;
using BlackJack.Implementation.GUI;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.TableActions
{
    /// <summary>
    /// Main game controller that orchestrates game flow.
    /// Coordinates between player, dealer, actions, and UI.
    /// </summary>
    public class GameLogic : IGameLogic
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private readonly IGraphicFactory _graphicFactory;
        private readonly IUserInputHandler _inputHandler;
        private readonly IPlayerFactory _playerFactory;
        private readonly IActionFactory _actionsFactory;

        private bool _isGameJustStarted = true;
        private GraphicInterface? _gameDisplay;
        private IActions _actions;
        private Stack<IPlayer>? _splitHands;
        private List<Card> _deckCards = [];

        /// <summary>
        /// Initializes a new instance of GameLogic.
        /// </summary>
        public GameLogic(IDealer dealer,
            IGraphicFactory graphicFactory, IUserInputHandler inputHandler,
            IPlayerFactory playerFactory, IActionFactory actionsFactory)
        {
            _player = playerFactory.Create() ?? throw new ArgumentNullException(nameof(playerFactory));
            _dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            _graphicFactory = graphicFactory ?? throw new ArgumentNullException(nameof(graphicFactory));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _actionsFactory = actionsFactory ?? throw new ArgumentNullException(nameof(actionsFactory));
            _actions = _actionsFactory.Create(_player, dealer, graphicFactory, playerFactory);
        }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        public void BeginGame()
        {
            GameDisplay.DisplayWelcomeScreen();

            int roundNumber = 1;
            InitializeRound();

            while (true)
            {
                GameDisplay.DisplayRoundSeparator(roundNumber);
                PlayMainCycle();

                if (IDealer.EndTheGame)
                {
                    GameDisplay.DisplayGoodbyeScreen(_player.Money);
                    break;
                }

                roundNumber++;
                InitializeRound();
                _isGameJustStarted = true;
            }
        }

        /// <summary>
        /// Prepares the game for a new round: clears hands, shuffles, deals cards, and displays initial state.
        /// </summary>
        private void InitializeRound()
        {
            _player.Hand.PairCards.Clear();
            _dealer.Hand.PairCards.Clear();
            if (_deckCards.Count > 0)
                _dealer.Refresh();

            _deckCards = _dealer.Shuffle();
            if (_deckCards.Count < GameConstants.MinCardsToPlay)
                throw new InvalidOperationException("Not enough cards to initialize the game.");

            _player.Hand.PairCards.Add(_deckCards[GameConstants.PlayerFirstCardIndex]);
            _player.Hand.PairCards.Add(_deckCards[GameConstants.PlayerSecondCardIndex]);
            _dealer.Hand.PairCards.Add(_deckCards[GameConstants.DealerFirstCardIndex]);
            _dealer.Hand.PairCards.Add(_deckCards[GameConstants.DealerSecondCardIndex]);

            // Display initial state
            _gameDisplay = (GraphicInterface)_graphicFactory.Create(_player, _dealer, true);
            _gameDisplay.IsDoubleNeeded = true;
        }

        /// <summary>
        /// Main game loop for a single round.
        /// </summary>
        private void PlayMainCycle()
        {
            while (true)
            {
                if (_isGameJustStarted)
                {
                    ProcessInitialBet();
                    _isGameJustStarted = false;

                    if (Conditions.IsBlackJack(_player.Hand))
                    {
                        HandlePlayerBlackjack();
                        return;
                    }
                }

                PlayerAction action = GetPlayerAction();

                if (!IsActionValid(action))
                    continue;

                if (ProcessPlayerAction(action))
                    return;
            }
        }

        /// <summary>
        /// Processes the split action and plays multiple hands.
        /// </summary>
        private void PlaySplitHands()
        {
            while (_splitHands?.Count > 0)
            {
                IPlayer splitPlayer = _splitHands.Pop();
                int handNumber = _splitHands.Count + 1;

                PlaySplitHandCycle(splitPlayer, handNumber);
            }
        }

        /// <summary>
        /// Plays a single split hand.
        /// </summary>
        private void PlaySplitHandCycle(IPlayer splitPlayer, int handNumber)
        {
            _actions = _actionsFactory.Create(_player, _dealer, _graphicFactory, _playerFactory);
            Console.WriteLine($"HAND {handNumber}");

            _gameDisplay = (GraphicInterface)_graphicFactory.Create(splitPlayer, _dealer, true);
            _gameDisplay.Print();

            bool handFinished = false;

            while (!handFinished)
            {
                PlayerAction action = GetPlayerAction();

                handFinished = ProcessSplitHandAction(action);
            }
        }

        /// <summary>
        /// Processes initial bet, adjusts ace if needed, and updates the menu options.
        /// </summary>
        private void ProcessInitialBet()
        {
            if (_gameDisplay == null)
                throw new InvalidOperationException("Game display not initialized. Call InitializeRound first.");

            decimal bet = _inputHandler.ReadBet(_player.Money);
            _player.Bet = bet;

            HandProcessor.ProcessCards(_player.Hand);

            bool isAPair = _player.Hand.PairCards[0].Title[0]
                == _player.Hand.PairCards[GameConstants.PlayerSecondCardIndex].Title[0];

            _gameDisplay.IsSplitNeeded = isAPair;
            _gameDisplay.IsDoubleNeeded = true;
            _gameDisplay.Print();
        }

        private void HandlePlayerBlackjack()
        {
            _player.ChangeMoney(_player.Bet * GameConstants.BlackjackPayout);

            var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
            display.WinMessage = "You win! You have a Black Jack!\n";
            display.Print();
        }

        private PlayerAction GetPlayerAction()
        {
            char input = _inputHandler.ReadAction();
            return (PlayerAction)input;
        }

        private bool IsActionValid(PlayerAction action)
        {
            if (action == PlayerAction.Double && (_gameDisplay == null || !_gameDisplay.IsDoubleNeeded))
                return false;

            if (action == PlayerAction.Split && (_gameDisplay == null || !_gameDisplay.IsSplitNeeded))
                return false;

            return true;
        }

        private bool ProcessPlayerAction(PlayerAction action)
        {
            // Execute action and interpret ActionResult for UI updates and round flow
            switch (action)
            {
                case PlayerAction.Hit:
                    {
                        var result = _actions.Hit();
                        if (result.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(result.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = result.Message;
                                display.Print();
                            }
                            else if (result.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (result.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                case PlayerAction.Stand:
                    {
                        var result = _actions.Stand();
                        if (result.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(result.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = result.Message;
                                display.Print();
                            }
                            else if (result.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (result.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                case PlayerAction.Double:
                    {
                        var result = _actions.Double();
                        if (result.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(result.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = result.Message;
                                display.Print();
                            }
                            else if (result.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (result.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                case PlayerAction.Split:
                    return HandleSplit();
                case PlayerAction.Exit:
                    return ExitGame();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Handles the split action: creates split hands, plays them, and combines money.
        /// </summary>
        private bool HandleSplit()
        {
            _splitHands = new Stack<IPlayer>();
            var splitResult = _actions.Split(_splitHands);
            if (splitResult.NeedRedraw)
                _gameDisplay?.Print();

            _player.Hand.PairCards.Clear();

            PlaySplitHands();

            decimal totalMoney = _player.Money;

            while (_splitHands?.Count > 0)
            {
                totalMoney += _splitHands.Pop().Money;
            }
            _player.Money = totalMoney;

            _actions = _actionsFactory.Create(_player, _dealer, _graphicFactory, _playerFactory);
            _gameDisplay = (GraphicInterface)_graphicFactory.Create(_player, _dealer, true);

            return false;
        }

        private bool ProcessSplitHandAction(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.Exit:
                    return ExitGame();
                case PlayerAction.Hit:
                    {
                        var r = _actions.Hit();
                        if (r.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(r.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = r.Message;
                                display.Print();
                            }
                            else if (r.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (r.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                case PlayerAction.Stand:
                    {
                        var r = _actions.Stand();
                        if (r.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(r.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = r.Message;
                                display.Print();
                            }
                            else if (r.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (r.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                case PlayerAction.Double when _gameDisplay?.IsDoubleNeeded == true:
                    {
                        var r = _actions.Double();
                        if (r.RoundEnded)
                        {
                            if (!string.IsNullOrEmpty(r.Message))
                            {
                                var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
                                display.WinMessage = r.Message;
                                display.Print();
                            }
                            else if (r.NeedRedraw)
                            {
                                _gameDisplay?.Print();
                            }

                            return true;
                        }

                        if (r.NeedRedraw) _gameDisplay?.Print();
                        return false;
                    }
                default:
                    return false;
            }
        }

        private bool ExitGame()
        {
            GameDisplay.DisplayGoodbyeScreen(_player.Money);
            Environment.Exit(0);
            return true;
        }
    }
}
