using BlackJack.Interfaces;

namespace BlackJack.Implementation.Entities
{
    public class Dealer : IDealer
    {
        private readonly Deck playDeck;

        private List<Card> mainCards;

        private List<Card> _readyCards;
        public List<Card> ReadyCards { get => _readyCards; set => _readyCards = value ?? []; }

        private readonly Random rand = new();

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
            _readyCards.Clear();
            mainCards.Clear();
            mainCards = [.. playDeck.deck];

            for (int i = 0; i < 4 && mainCards.Count > 0; i++)
            {
                int index = rand.Next(mainCards.Count);
                _readyCards.Add(mainCards[index]);
                mainCards.RemoveAt(index);
            }

            return [.. _readyCards];
        }

        public Card Pull()
        {
            if (mainCards.Count == 0)
                throw new InvalidOperationException("No more cards in the deck.");

            int index = rand.Next(mainCards.Count);
            var card = mainCards[index];
            mainCards.RemoveAt(index);
            return card;
        }

        public void Refresh()
        {
            _readyCards.Clear();
            mainCards.Clear();
            mainCards = [.. playDeck.deck];
            IsHitted = false;

            foreach (var item in playDeck.deck)
            {
                item.IsHidden = false;
            }
        }
    }
}
