namespace BlackJack.Interfaces
{
    /// <summary>
    /// Defines the contract for handling user input.
    /// Allows for easy replacement with different input methods (console, GUI, etc.).
    /// </summary>
    public interface IUserInputHandler
    {
        /// <summary>
        /// Reads a single character representing a player action.
        /// </summary>
        /// <returns>The character entered by the user.</returns>
        char ReadAction();

        /// <summary>
        /// Reads a decimal bet amount from the player.
        /// </summary>
        /// <returns>The bet amount entered by the user.</returns>
        decimal ReadBet();
    }
}
