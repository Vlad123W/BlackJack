using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class Deck
    {
        public readonly List<Card> deck =
        [
            new(){ Title = "2♥", Cost = 2},
            new(){ Title = "2♦", Cost = 2},
            new(){ Title = "2♠", Cost = 2},
            new(){ Title = "2♣", Cost = 2},

            new(){ Title = "3♥", Cost = 3},
            new(){ Title = "3♦", Cost = 3},
            new(){ Title = "3♠", Cost = 3},
            new(){ Title = "3♣", Cost = 3},

            new(){ Title = "4♥", Cost = 4},
            new(){ Title = "4♦", Cost = 4},
            new(){ Title = "4♠", Cost = 4},
            new(){ Title = "4♣", Cost = 4},

            new(){ Title = "5♥", Cost = 5},
            new(){ Title = "5♦", Cost = 5},
            new(){ Title = "5♠", Cost = 5},
            new(){ Title = "5♣", Cost = 5},

            new(){ Title = "6♥", Cost = 6},
            new(){ Title = "6♦", Cost = 6},
            new(){ Title = "6♠", Cost = 6},
            new(){ Title = "6♣", Cost = 6},

            new(){ Title = "7♥", Cost = 7},
            new(){ Title = "7♦", Cost = 7},
            new(){ Title = "7♠", Cost = 7},
            new(){ Title = "7♣", Cost = 7},

            new(){ Title = "8♥", Cost = 8},
            new(){ Title = "8♦", Cost = 8},
            new(){ Title = "8♠", Cost = 8},
            new(){ Title = "8♣", Cost = 8},

            new(){ Title = "9♥", Cost = 9},
            new(){ Title = "9♦", Cost = 9},
            new(){ Title = "9♠", Cost = 9},
            new(){ Title = "9♣", Cost = 9},

            new(){ Title = "10♥", Cost = 10},
            new(){ Title = "10♦", Cost = 10},
            new(){ Title = "10♠", Cost = 10},
            new(){ Title = "10♣", Cost = 10},

            new(){ Title = "J♥", Cost = 10},
            new(){ Title = "J♦", Cost = 10},
            new(){ Title = "J♠", Cost = 10},
            new(){ Title = "J♣", Cost = 10},

            new(){ Title = "Q♥", Cost = 10},
            new(){ Title = "Q♦", Cost = 10},
            new(){ Title = "Q♠", Cost = 10},
            new(){ Title = "Q♣", Cost = 10},

            new(){ Title = "K♥", Cost = 10},
            new(){ Title = "K♦", Cost = 10},
            new(){ Title = "K♠", Cost = 10},
            new(){ Title = "K♣", Cost = 10},

            new(){ Title = "A♥", Cost = 11},
            new(){ Title = "A♦", Cost = 11},
            new(){ Title = "A♠", Cost = 11},
            new(){ Title = "A♣", Cost = 11},
        ];

        public void PrintDeck()
        {
            foreach (var item in deck)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
