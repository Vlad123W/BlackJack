using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create() => new Player();
    }
}
