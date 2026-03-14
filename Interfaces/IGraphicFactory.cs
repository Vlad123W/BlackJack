namespace BlackJack.Interfaces
{
    /// <summary>
    /// Factory interface for creating UI components.
    /// Allows for dependency injection and easy testing.
    /// </summary>
    public interface IGraphicFactory
    {
        /// <summary>
        /// Creates a new graphic interface instance.
        /// </summary>
        /// <param name="player">The player's game state.</param>
        /// <param name="dealer">The dealer's game state.</param>
        /// <param name="hideDealerSecond">Whether to hide the dealer's second card.</param>
        /// <returns>A new IGraphicInterface implementation.</returns>
        IGraphicInterface Create(IPlayer player, IDealer dealer, bool hideDealerSecond = true);
    }
}
