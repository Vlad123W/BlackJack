using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Implementation
{
    public class Hand : IHand
    {
        private List<Card> _paircards = [];
        public List<Card> PairCards { get => _paircards; set => _paircards = value; }
        
        public void Show()
        {
            foreach (var item in PairCards)
            {
                Console.Write(item.Title + " ");
            }
 
            Console.WriteLine();
        }

        public int GetScore() => _paircards.Sum(x => x.Cost);

        public void Clear() => _paircards.Clear();
    }
}
