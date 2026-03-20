using BlackJack.Implementation.TableActions;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.Factories
{
    public class ActionFactory : IActionFactory
    {
        public IActions Create(IPlayer player, IDealer dealer, IGraphicFactory graphicFactory, IPlayerFactory playerFactory = null)
        {
            return new Actions(player, dealer, graphicFactory, playerFactory);
        }
    }
}
