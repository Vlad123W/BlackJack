using BlackJack.Interfaces;

namespace BlackJack.Implementation
{
    /// <summary>
    /// Factory for creating GraphicInterface instances.
    /// Decouples UI creation from the rest of the application.
    /// </summary>
    public class GraphicFactory : IGraphicFactory
    {
        /// <summary>
        /// Creates a new GraphicInterface instance.
        /// </summary>
        /// <param name="player">The player's game state.</param>
        /// <param name="dealer">The dealer's game state.</param>
        /// <param name="hideDealerSecond">Whether to hide the dealer's second card.</param>
        /// <returns>A new IGraphicInterface implementation.</returns>
        public IGraphicInterface Create(IPlayer player, IDealer dealer, bool hideDealerSecond = true)
        {
            return new GraphicInterface(player, dealer, hideDealerSecond);
        }
    }
}
