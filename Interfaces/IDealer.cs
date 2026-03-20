using BlackJack.Implementation.Entities;

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
