using BlackJack.Implementation.Data;
using BlackJack.Implementation.GUI;
using BlackJack.Interfaces;
using System;
using System.Collections.Generic;

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

        public event IActions.Notify? Hitted;
        public event IActions.Notify? GameEnded;

        /// <summary>
        /// Initializes a new instance of the Actions class.
        /// </summary>
        /// <param name="player">The player performing actions.</param>
        /// <param name="dealer">The dealer object.</param>
        /// <param name="graphicFactory">Factory for creating UI components.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public Actions(IPlayer player, IDealer dealer, IGraphicFactory graphicFactory)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            _graphicFactory = graphicFactory ?? throw new ArgumentNullException(nameof(graphicFactory));
        }

        /// <summary>
        /// Player requests another card.
        /// </summary>
        /// <returns>True if the round should end (blackjack or bust), false otherwise.</returns>
        public bool Hit()
        {
            IDealer.IsHitted = true;
            _player.Hand.PairCards.Add(_dealer.Pull());

            if (Conditions.IsBlackJack(_player.Hand))
            {
                HandlePlayerBlackjack();
                return true;
            }

            if (Conditions.IsBusted(_player.Hand))
            {
                HandlePlayerBust();
                return true;
            }

            Hitted?.Invoke();
            return false;
        }

        /// <summary>
        /// Player ends their turn and dealer plays.
        /// </summary>
        /// <returns>Always true (round always ends).</returns>
        public bool Stand()
        {
            ExecuteDealerTurn();

            if (Conditions.IsBusted(_dealer.Hand))
            {
                HandleDealerBust();
                return true;
            }

            EvaluateAndDisplayOutcome();
            return true;
        }

        /// <summary>
        /// Player doubles their bet and takes one more card, then stands automatically.
        /// </summary>
        /// <returns>Always true (round always ends).</returns>
        public bool Double()
        {
            _player.Bet *= GameConstants.DoubleMultiplier;
            Hit();
            Stand();
            return true;
        }

        /// <summary>
        /// Player splits a pair into two separate hands.
        /// </summary>
        /// <param name="splitHands">Stack to store the split hands.</param>
        /// <exception cref="ArgumentNullException">Thrown when splitHands is null.</exception>
        public void Split(Stack<IPlayer> splitHands)
        {
            ArgumentNullException.ThrowIfNull(splitHands);

            if (!CanSplitHand())
                return;

            var splitPlayers = CreateSplitHands();
            splitHands.Push(splitPlayers.Player1);
            splitHands.Push(splitPlayers.Player2);
        }

        private void HandlePlayerBlackjack()
        {
            decimal payout = _player.Bet * GameConstants.BlackjackPayout;
            _player.ChangeMoney(payout);

            DisplayResult("You win! Black Jack!\n");
        }

        private void HandlePlayerBust()
        {
            _player.ChangeMoney(-_player.Bet);
            DisplayResult("You lost! You're busted!\n");
        }

        private void HandleDealerBust()
        {
            _player.ChangeMoney(_player.Bet);
            DisplayResult("You win! Dealer is busted!\n");
            GameEnded?.Invoke();
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

        private void EvaluateAndDisplayOutcome()
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

            DisplayResult(outcome);
            GameEnded?.Invoke();
        }

        private void DisplayResult(string message)
        {
            var display = (GraphicInterface)_graphicFactory.Create(_player, _dealer, false);
            display.WinMessage = message;
            display.Print();
        }

        private bool CanSplitHand()
        {
            return _player.Hand.PairCards.Count >= GameConstants.InitialCardsDealt;
        }

        private (IPlayer Player1, IPlayer Player2) CreateSplitHands()
        {
            var firstCard = _player.Hand.PairCards[GameConstants.PlayerFirstCardIndex];
            var secondCard = _player.Hand.PairCards[GameConstants.PlayerSecondCardIndex];
            _player.Hand.Clear();

            var hand1 = new Player();
            var hand2 = new Player();

            hand1.Hand.PairCards.Add(firstCard);
            hand2.Hand.PairCards.Add(secondCard);

            hand1.Bet = _player.Bet;
            hand2.Bet = _player.Bet;

            decimal startingMoney = _player.Money;
            hand1.Money = startingMoney;
            hand2.Money = startingMoney;

            hand1.Hand.PairCards.Add(_dealer.Pull());
            hand2.Hand.PairCards.Add(_dealer.Pull());

            return (hand1, hand2);
        }
    }
}
