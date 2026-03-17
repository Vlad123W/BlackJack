using BlackJack.Implementation.TableActions;
using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
