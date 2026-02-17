using BlackJack.Implementation;
using BlackJack.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new();
            services.AddSingleton<IPlayer, Player>();
            services.AddSingleton<IDealer, Dealer>();
            services.AddSingleton<IActions, Actions>();
            services.AddSingleton<IGameLogic, GameLogic>();

            ServiceProvider provider = services.BuildServiceProvider();
            
            provider.GetService<IGameLogic>()?.BeginGame();


        }
    }
}
