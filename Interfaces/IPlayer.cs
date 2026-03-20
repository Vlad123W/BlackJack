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
