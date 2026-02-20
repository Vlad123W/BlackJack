using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJack.Implementation
{
    public class Actions : IActions
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;

        private const int MaxStandScoreValue = 17;

        public event IActions.Notify? Hitted;
        public event IActions.Notify? GameEnded;

        private readonly IServiceProvider _provider;

        public Actions(IPlayer player, IDealer dealer, IServiceProvider provider)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public bool Hit()
        {
            IDealer.IsHitted = true;
            _player.Hand.PairCards.Add(_dealer.Pull());

            if (Conditions.IsBlackJack(_player.Hand))
            {
                // Blackjack: player gets 1.5x the bet profit (common rule)
                var payout = _player.Bet * 1.5m;
                _player.ChangeMoney(payout);

                var gi = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, true);
                gi.WinMessage = "You win! Black Jack!\n";

                gi.Print();
                return true;
            }

            if (Conditions.IsBusted(_player.Hand))
            {
                // Player loses their bet
                _player.ChangeMoney(-_player.Bet);

                var gi = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, true);
                gi.WinMessage = "You lost! You're busted!\n";

                gi.Print();
                return true;
            }

            Hitted?.Invoke();
            return false;
        }

        public bool Stand()
        {
            while (_dealer.Hand.GetScore() < MaxStandScoreValue && Conditions.CanHit(_dealer.Hand) && _dealer.Hand.PairCards.Count < 5)
            {
                _dealer.Hand.PairCards.Add(_dealer.Pull());
            }
            if (Conditions.IsBusted(_dealer.Hand))
            {
                // Player wins an amount equal to their bet
                _player.ChangeMoney(_player.Bet);

                var gi = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, false);
                gi.WinMessage = "You win! Dealer is busted!\n";

                gi.Print();
                GameEnded?.Invoke();
                return true;
            }

            var outcome = Conditions.EvaluateWinner(_player, _dealer);
            if (outcome.Contains("You win!"))
            {
                _player.ChangeMoney(_player.Bet);
            }
            else if (outcome.Contains("You lost!"))
            {
                _player.ChangeMoney(-_player.Bet);
            }

            var resultGi = ActivatorUtilities.CreateInstance<GraphicInterface>(_provider, _player, _dealer, false);
            resultGi.WinMessage = outcome;

            resultGi.Print();
            GameEnded?.Invoke();
            return true;
        }

        public bool Double()
        {
            _player.Bet *= 2m;
            Hit();
            Stand();
            return true;
        }

        public void Split(Stack<IPlayer> splitHands)
        {
            ArgumentNullException.ThrowIfNull(splitHands);

            if (_player.Hand.PairCards.Count < 2) return; 

            var firstCard = _player.Hand.PairCards[0];
            var secondCard = _player.Hand.PairCards[1];
            _player.Hand.Clear();

            var hand1 = new Player();
            var hand2 = new Player();

            hand1.Hand.PairCards.Add(firstCard);
            hand2.Hand.PairCards.Add(secondCard);

            hand1.Bet = _player.Bet;
            hand2.Bet = _player.Bet;

            var startingMoney = _player.Money;
            hand1.Money = startingMoney;
            hand2.Money = startingMoney;

            hand1.Hand.PairCards.Add(_dealer.Pull());
            hand2.Hand.PairCards.Add(_dealer.Pull());

            splitHands.Push(hand1);
            splitHands.Push(hand2);
        }
    }
}
