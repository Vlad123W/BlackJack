using BlackJack.Interfaces;

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPlayer player = new Player();
            IDealer dealer = new Dealer();

            IActions actions = new Actions(player, dealer);

            IGameLogic gameLogic = new GameLogic(player, dealer, actions);
            gameLogic.BeginGame();

            Console.WriteLine("Press any key....");
            Console.ReadKey();
        }
    }
}
