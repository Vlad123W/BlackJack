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

        public event IActions.Notify? Hitted;
        public event IActions.Notify? GameEnded;

        public Actions(IPlayer player, IDealer dealer)
        {
            _player = player;
            _dealer = dealer;
            Hitted += Actions_Hitted;
        }

        private void Actions_Hitted()
        {
            IGraphicInterface _gi = new GraphicInterface(_player, _dealer, [true]);
            _gi.Print();
        }

        public bool Hit()
        {
            IDealer.IsHitted = true;

            _player.Hand.PairCards.Add(_dealer.Pull());
            
            IGraphicInterface _gi;

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
            while (_dealer.Hand.GetScore() < _player.Hand.GetScore() && Conditions.CanHit(_dealer.Hand))
            {
                _dealer.Hand.PairCards.Add(_dealer.Pull());
            }

            if(Conditions.IsBusted(_dealer.Hand))
            {
                _player.ChangeMoney(_player.Bet * 2m);
                
                IGraphicInterface _gi;
                
                _gi = new GraphicInterface(_player, _dealer, [false])
                {
                    WinMessage = "You win! Dealer is busted!\n"
                };
                
                _gi.Print();

                GameEnded?.Invoke();

                return true;
            }
            else
            {

                IGraphicInterface gi = new GraphicInterface(_player, _dealer, [false])
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

        public bool Exit()
        {
            IDealer.EndTheGame = true;
            return true;
        }
      
        public void Split(ObservableCollection<IPlayer> splitHands)
        {
            Player hand1 = new();
            Player hand2 = new();

            hand1.Hand.PairCards.Add(_player.Hand.PairCards[0]);
            hand2.Hand.PairCards.Add(_player.Hand.PairCards[1]);

            hand1.Money = _player.Money;
            hand2.Money = _player.Money;

            hand1.Bet = _player.Bet;
            hand2.Bet = _player.Bet;

            splitHands.Add(hand1);
            splitHands.Add(hand2);
        }
    }
}
