namespace BlackJack.Interfaces
{
    /// <summary>
    /// Defines the contract for displaying game state and menu to the player.
    /// </summary>
    public interface IGraphicInterface
    {
        /// <summary>
        /// Gets or sets the outcome message to display.
        /// </summary>
        string WinMessage { get; set; }

        /// <summary>
        /// Gets the dynamically built menu string based on available actions.
        /// </summary>
        string MenuString { get; }

        /// <summary>
        /// Displays the current game state to the player.
        /// </summary>
        void Print();
    }
}
