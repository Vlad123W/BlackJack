using BlackJack.Implementation;
using BlackJack.Interfaces;
using Newtonsoft.Json;

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new();
            Dealer dealer = new();

            IActions actions = new Actions(player, dealer);

            IGameLogic gameLogic = new GameLogic(player, dealer, actions);
            gameLogic.BeginGame();

            Console.WriteLine("Press any key....");
            Console.ReadKey();
        }
    }
}
