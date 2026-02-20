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
            var services = new ServiceCollection();

            // Register implementations
            services.AddSingleton<IPlayer, Player>();
            services.AddSingleton<IDealer, Dealer>();

            // Actions and GameLogic depend on scoped/Transient services (IPlayer/IDealer are singletons here)
            services.AddTransient<IActions, Actions>();
            services.AddTransient<IGameLogic, GameLogic>();

            var provider = services.BuildServiceProvider();

            // Resolve GameLogic and run
            using var scope = provider.CreateScope();
            var game = scope.ServiceProvider.GetRequiredService<IGameLogic>();
            game.BeginGame();
        }
    }
}
