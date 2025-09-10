using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Implementation
{
    public class Dealer : IDealer
    {
        private readonly Deck playDeck;
        
        private List<Card> mainCards;

        private List<Card> _readyCards;
        public List<Card> ReadyCards { get => _readyCards; set => _readyCards = value; }
       
        private Random? rand;

        private IHand? _hand = new Hand();
        public IHand Hand { get => _hand; set => _hand = value; }
       
        private static bool _isHitted;
        public static bool IsHitted { get => _isHitted; set => _isHitted = value; }

        private static bool _endTheGame;
        public static bool EndTheGame { get => _endTheGame; set => _endTheGame = value; }

        public Dealer()
        {
            playDeck = new();
            mainCards = [.. playDeck.deck]; 
            _readyCards = [];
        }

        public List<Card> Shuffle()
        {
            rand = new();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(mainCards.Count);
                _readyCards.Add(mainCards[index]);
                mainCards.Remove(mainCards[index]);
            }

            return [.. _readyCards];
        }

        public Card Pull()
        {
            rand = new();

            int index = rand.Next(mainCards.Count);

            return mainCards[index];
        }

        public void Refresh()
        {
            _readyCards.Clear();
            mainCards.Clear();
            mainCards = [.. playDeck.deck];
            IsHitted = false;

            foreach (var item in playDeck.deck)
            {
                if(item.IsHidden)
                {
                    item.IsHidden = false;
                }
            }
        }
    }
}
