using BlackJack.Implementation.Entities;
using BlackJack.Implementation.Factories;
using BlackJack.Implementation.TableActions;
using BlackJack.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJack
{
    /// <summary>
    /// Entry point for the BlackJack game application.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var game = scope.ServiceProvider.GetRequiredService<IGameLogic>();
            game.BeginGame();
        }

        /// <summary>
        /// Configures all application services and dependencies.
        /// </summary>
        private static void ConfigureServices(ServiceCollection services)
        {
            // Game state objects
            services.AddSingleton<IPlayerFactory, PlayerFactory>();
            services.AddSingleton<IActionFactory, ActionFactory>();
            services.AddTransient<IPlayer, Player>();
            services.AddSingleton<IDealer, Dealer>();

            // UI components
            services.AddSingleton<IGraphicFactory, GraphicFactory>();
            services.AddSingleton<IUserInputHandler, ConsoleUserInputHandler>();

            // Game logic
            services.AddTransient<IActions, Actions>();
            services.AddTransient<IGameLogic, GameLogic>();
        }
    }
}
