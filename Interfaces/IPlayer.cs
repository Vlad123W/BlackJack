using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Interfaces
{
    public interface IPlayer
    {
        void ChangeMoney(decimal income);
        decimal Money { get; set; }
        decimal Bet { get; set; }
        IHand Hand { get; set; }
    }
}
