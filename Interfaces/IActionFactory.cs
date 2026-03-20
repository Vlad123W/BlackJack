namespace BlackJack.Interfaces
{
    public interface IActionFactory
    {
        IActions Create(IPlayer player, IDealer dealer, IGraphicFactory graphicFactory, IPlayerFactory playerFactory = null);
    }
}
