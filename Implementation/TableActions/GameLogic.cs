using BlackJack.Implementation.Data;
using BlackJack.Implementation.Entities;
using BlackJack.Implementation.GUI;
using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private IActions _actions;
        private readonly IGraphicFactory _graphicFactory;
        private readonly IUserInputHandler _inputHandler;
        private readonly IPlayerFactory _playerFactory;

        private bool _isGameJustStarted = true;
        private GraphicInterface? _gameDisplay;
        private Stack<IPlayer>? _splitHands;
        private List<Card> _deckCards = [];

        /// <summary>
        /// Initializes a new instance of GameLogic.
        /// </summary>
        public GameLogic(IPlayer player, IDealer dealer, IActions actions, 
            IGraphicFactory graphicFactory, IUserInputHandler inputHandler, IPlayerFactory playerFactory)
        {
            _player = player;
            _dealer = dealer;
            _actions = actions;
            _graphicFactory = graphicFactory ?? throw new ArgumentNullException(nameof(graphicFactory));
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));

            _actions.Hitted += OnPlayerHit;
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
        /// Prepares the game for a new round.
        /// </summary>
        private void InitializeRound()
        {
            ClearPreviousHands();
            ShuffleAndDeal();
            DisplayInitialState();
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
            _actions = new Actions(splitPlayer, _dealer, _graphicFactory);
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

        private void ClearPreviousHands()
        {
            _player.Hand.PairCards.Clear();
            _dealer.Hand.PairCards.Clear();

            if (_deckCards.Count > 0)
                _dealer.Refresh();
        }

        private void ShuffleAndDeal()
        {
            _deckCards = _dealer.Shuffle();

            if (_deckCards.Count < GameConstants.MinCardsToPlay)
                throw new InvalidOperationException("Not enough cards to initialize the game.");

            _player.Hand.PairCards.Add(_deckCards[GameConstants.PlayerFirstCardIndex]);
            _player.Hand.PairCards.Add(_deckCards[GameConstants.PlayerSecondCardIndex]);

            _dealer.Hand.PairCards.Add(_deckCards[GameConstants.DealerFirstCardIndex]);
            _dealer.Hand.PairCards.Add(_deckCards[GameConstants.DealerSecondCardIndex]);
        }

        private void DisplayInitialState()
        {
            _gameDisplay = (GraphicInterface)_graphicFactory.Create(_player, _dealer, true);
            _gameDisplay.IsDoubleNeeded = true;
        }

        private void ProcessInitialBet()
        {
            decimal bet = _inputHandler.ReadBet();
            _player.Bet = bet;

            AdjustAceIfNeeded();
            UpdateMenuForInitialHand();
            _gameDisplay?.Print();
        }

        private void AdjustAceIfNeeded()
        {
            if (_player.Hand.PairCards.All(x => x.Title.Contains('A')))
            {
                _player.Hand.PairCards[0].Cost = 1;
            }
        }

        private void UpdateMenuForInitialHand()
        {
            bool isAPair = _player.Hand.PairCards[0].Title[0] == _player.Hand.PairCards[1].Title[0];

            _gameDisplay!.IsSplitNeeded = isAPair;
            _gameDisplay.IsDoubleNeeded = true;
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
            // Check if Double is attempted when not allowed
            if (action == PlayerAction.Double && (_gameDisplay == null || !_gameDisplay.IsDoubleNeeded))
                return false;

            // Check if Split is attempted when not allowed
            if (action == PlayerAction.Split && (_gameDisplay == null || !_gameDisplay.IsSplitNeeded))
                return false;

            return true;
        }

        private bool ProcessPlayerAction(PlayerAction action)
        {
            return action switch
            {
                PlayerAction.Hit => _actions.Hit(),
                PlayerAction.Stand => _actions.Stand(),
                PlayerAction.Double => _actions.Double(),
                PlayerAction.Split => ProcessSplit(),
                PlayerAction.Exit => ExitGame(),
                _ => false
            };
        }

        private bool ProcessSplitHandAction(PlayerAction action)
        {
            return action switch
            {
                PlayerAction.Exit => ExitGame(),
                PlayerAction.Hit => _actions.Hit(),
                PlayerAction.Stand => _actions.Stand(),
                PlayerAction.Double when _gameDisplay?.IsDoubleNeeded == true => _actions.Double(),
                _ => false
            };
        }

        private bool ProcessSplit()
        {
            _splitHands = new Stack<IPlayer>();
            _actions.Split(_splitHands);

            PlaySplitHands();

            CombineSplitHandsMoney();

            _actions = new Actions(_player, _dealer, _graphicFactory, _playerFactory);
            _gameDisplay = (GraphicInterface)_graphicFactory.Create(_player, _dealer, true);

            return false;
        }

        private void CombineSplitHandsMoney()
        {
            decimal totalMoney = _player.Money;

            while (_splitHands?.Count > 0)
            {
                totalMoney += _splitHands.Pop().Money;
            }

            _player.Money = totalMoney;
        }

        private bool ExitGame()
        {
            GameDisplay.DisplayGoodbyeScreen(_player.Money);
            Environment.Exit(0);
            return true;
        }

        private void OnPlayerHit()
        {
            _gameDisplay?.Print();
        }
    }
}
