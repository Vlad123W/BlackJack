using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Implementation
{
    public class Player : IPlayer
    {
        private IHand _hand = new Hand();
        public IHand Hand { get => _hand; set => _hand = value; }

        private decimal _money = 1000;
        public decimal Money { get => _money; set => _money = value; }
        
        private decimal _bet;
        public decimal Bet
        {
            get => _bet;

            set
            {
                if (value < 0)
                {
                    _bet = -value;
                    return;
                }
                else if (value == 0)
                {
                    _bet = 10;
                    return;
                }

                _bet = value;
            }
        }

        public void ChangeMoney(decimal income) => _money += income;
    }
}
