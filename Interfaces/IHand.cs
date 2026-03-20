using BlackJack.Implementation.Entities;

namespace BlackJack.Interfaces
{
    public interface IHand
    {
        void Show();
        int GetScore();
        void Clear();
        IList<Card> PairCards { get; set; }
    }
}