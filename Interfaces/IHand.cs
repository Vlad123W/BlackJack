namespace BlackJack.Interfaces
{
    public interface IHand
    {
        void Show();
        int GetScore();
        void Clear();
        List<Card> PairCards { get; set; }
    }
}