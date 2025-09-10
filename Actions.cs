using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Actions : IActions
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;

        private const int MAX_STAND_SCORE_VALUE = 17;
        public event IActions.Notify? Hitted;
        public event IActions.Notify? GameEnded;

        public Actions(IPlayer player, IDealer dealer)
        {
            _player = player;
            _dealer = dealer;
        }

        public bool Hit()
        {
            IDealer.IsHitted = true;

            _player.Hand.PairCards.Add(_dealer.Pull());
            
            GraphicInterface _gi;

            if(Conditions.IsBlackJack(_player.Hand))
            {
                _player.ChangeMoney(_player.Bet * 2m);

                _gi = new GraphicInterface(_player, _dealer, [true])
                {
                    WinMessage = "You win! BalckJack!\n"
                };

                _gi.Print();

                return true;
            }

            if(Conditions.IsBusted(_player.Hand))
            {
                _player.ChangeMoney(-_player.Bet);

                _gi = new GraphicInterface(_player, _dealer, [true])
                {
                    WinMessage = "You lost! You're busted!\n"
                };

                _gi.Print();

                return true;
            }

            Hitted?.Invoke();
           
            return false;
        }

        public bool Stand()
        {
            while (_dealer.Hand.GetScore() < MAX_STAND_SCORE_VALUE && Conditions.CanHit(_dealer.Hand) && _dealer.Hand.PairCards.Count < 5)
            {
                _dealer.Hand.PairCards.Add(_dealer.Pull());
            }

            if(Conditions.IsBusted(_dealer.Hand))
            {
                _player.ChangeMoney(_player.Bet * 2m);
                
                GraphicInterface _gi = new(_player, _dealer, [false])
                {
                    WinMessage = "You win! Dealer is busted!\n"
                };
                
                _gi.Print();

                GameEnded?.Invoke();

                return true;
            }
            else
            {

                GraphicInterface gi = new(_player, _dealer, [false])
                {
                    WinMessage = Conditions.EvaluateWinner(_player, _dealer)
                };

                gi.Print();
                GameEnded?.Invoke();
            }

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
            Player hand1 = new();
            Player hand2 = new();

            hand1.Hand.PairCards.Add(_player.Hand.PairCards[0]);
            hand2.Hand.PairCards.Add(_player.Hand.PairCards[1]);

            hand1.Money = _player.Money;
            hand2.Money = _player.Money;

            hand1.Bet = _player.Bet;
            hand2.Bet = _player.Bet;

            splitHands.Push(hand1);
            splitHands.Push(hand2);
        }
    }
}
