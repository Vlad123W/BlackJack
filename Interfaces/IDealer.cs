using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Interfaces
{
    public interface IDealer
    {
        List<Card> Shuffle();
        Card Pull();
        void Refresh();

        List<Card> ReadyCards { get; set; }

        static bool IsHitted { get; set; }
        static bool EndTheGame { get; set; }

        IHand Hand { get; set; }
    }
}
