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
        /// Reads a decimal bet amount from the player with optional balance validation.
        /// </summary>
        /// <param name="playerMoney">The player's current balance. Defaults to MaxValue (no limit).</param>
        /// <returns>The bet amount entered by the user.</returns>
        decimal ReadBet(decimal playerMoney = decimal.MaxValue);
    }
}
