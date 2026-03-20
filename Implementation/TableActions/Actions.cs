using BlackJack.Implementation.Data;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.TableActions
{
    /// <summary>
    /// Handles player actions during the game (Hit, Stand, Double, Split).
    /// Encapsulates the game logic for each action and its consequences.
    /// </summary>
    public class Actions : IActions
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private readonly IGraphicFactory _graphicFactory;
        private readonly IPlayerFactory? _playerFactory;

        /// <summary>
        /// Initializes a new instance of the Actions class.
        /// </summary>
        /// <param name="player">The player performing actions.</param>
        /// <param name="dealer">The dealer object.</param>
        /// <param name="graphicFactory">Factory for creating UI components.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public Actions(IPlayer player, IDealer dealer, IGraphicFactory graphicFactory, IPlayerFactory? playerFactory = null)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            _graphicFactory = graphicFactory ?? throw new ArgumentNullException(nameof(graphicFactory));
            _playerFactory = playerFactory;
        }

        /// <summary>
        /// Player requests another card.
        /// </summary>
        /// <returns>True if the round should end (blackjack or bust), false otherwise.</returns>
        public ActionResult Hit()
        {
            IDealer.IsHitted = true;
            _player.Hand.PairCards.Add(_dealer.Pull());

            if (Conditions.IsBlackJack(_player.Hand))
            {
                HandlePlayerBlackjack();
                return new ActionResult(RoundEnded: true, NeedRedraw: true, Message: "You win! Black Jack!\n");
            }
            else if (Conditions.IsBusted(_player.Hand) && !HandProcessor.ProcessCards(_player.Hand))
            {
                HandlePlayerBust();
                return new ActionResult(RoundEnded: true, NeedRedraw: true, Message: "You lost! You're busted!\n");
            }

            return new ActionResult(RoundEnded: false, NeedRedraw: true);
        }

        /// <summary>
        /// Player ends their turn and dealer plays.
        /// </summary>
        /// <returns>Always true (round always ends).</returns>
        public ActionResult Stand()
        {
            ExecuteDealerTurn();

            if (Conditions.IsBusted(_dealer.Hand))
            {
                HandleDealerBust();
                return new ActionResult(RoundEnded: true, NeedRedraw: true, Message: "You win! Dealer is busted!\n");
            }

            var outcome = EvaluateOutcome();
            return new ActionResult(RoundEnded: true, NeedRedraw: true, Message: outcome);
        }

        /// <summary>
        /// Player doubles their bet and takes one more card, then stands automatically.
        /// </summary>
        /// <returns>Always true (round always ends).</returns>
        public ActionResult Double()
        {
            _player.Bet *= GameConstants.DoubleMultiplier;
            var hitResult = Hit();
            if (hitResult.RoundEnded)
                return hitResult;

            return Stand();
        }

        /// <summary>
        /// Player splits a pair into two separate hands.
        /// </summary>
        /// <param name="splitHands">Stack to store the split hands.</param>
        /// <exception cref="ArgumentNullException">Thrown when splitHands is null.</exception>
        public ActionResult Split(Stack<IPlayer> splitHands)
        {
            ArgumentNullException.ThrowIfNull(splitHands);

            if (!CanSplitHand())
                return new ActionResult(RoundEnded: false, NeedRedraw: false, Message: "Cannot split");

            var (Player1, Player2) = CreateSplitHands();
            splitHands.Push(Player1);
            splitHands.Push(Player2);

            return new ActionResult(RoundEnded: false, NeedRedraw: true, Message: "Split performed");
        }

        private void HandlePlayerBlackjack()
        {
            decimal payout = _player.Bet * GameConstants.BlackjackPayout;
            _player.ChangeMoney(payout);
        }

        private void HandlePlayerBust()
        {
            _player.ChangeMoney(-_player.Bet);
        }

        private void HandleDealerBust()
        {
            _player.ChangeMoney(_player.Bet);
        }

        private void ExecuteDealerTurn()
        {
            while (_dealer.Hand.GetScore() < GameConstants.DealerStandScore &&
                   Conditions.CanHit(_dealer.Hand) &&
                   _dealer.Hand.PairCards.Count < GameConstants.MaxCardsInHand)
            {
                _dealer.Hand.PairCards.Add(_dealer.Pull());
            }
        }

        private string EvaluateOutcome()
        {
            string outcome = Conditions.EvaluateWinner(_player, _dealer);

            if (outcome.Contains("You win!"))
            {
                _player.ChangeMoney(_player.Bet);
            }
            else if (outcome.Contains("You lost!"))
            {
                _player.ChangeMoney(-_player.Bet);
            }

            return outcome;
        }

        private bool CanSplitHand()
        {
            return _player.Hand.PairCards.Count >= GameConstants.InitialCardsDealt;
        }

        private (IPlayer Player1, IPlayer Player2) CreateSplitHands()
        {
            if (_playerFactory == null) throw new ArgumentNullException(nameof(_playerFactory));

            var firstCard = _player.Hand.PairCards[GameConstants.PlayerFirstCardIndex];
            var secondCard = _player.Hand.PairCards[GameConstants.PlayerSecondCardIndex];

            var hand1 = _playerFactory.Create();
            var hand2 = _playerFactory.Create();

            hand1.Hand.PairCards.Add(firstCard);
            hand2.Hand.PairCards.Add(secondCard);

            hand1.Bet = _player.Bet;
            hand2.Bet = _player.Bet;

            // Deduct both bets from the original player's money
            _player.ChangeMoney(-_player.Bet);

            // Each split hand starts with half the remaining money, but includes their own bet
            decimal remainingMoney = _player.Money;
            hand1.Money = remainingMoney;
            hand2.Money = remainingMoney;

            hand1.Hand.PairCards.Add(_dealer.Pull());
            hand2.Hand.PairCards.Add(_dealer.Pull());

            return (hand1, hand2);
        }
    }
}
