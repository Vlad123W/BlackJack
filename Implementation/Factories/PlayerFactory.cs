using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Implementation.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create() => new Player();
    }
}
