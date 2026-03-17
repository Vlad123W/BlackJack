using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Implementation.Entities
{
    public class Hand : IHand
    {
        // Initialize with an empty list using standard syntax
        private List<Card> _paircards = [];
        public IList<Card> PairCards { get => _paircards; set => _paircards = value as List<Card> ?? [.. value ?? Enumerable.Empty<Card>()]; }

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
