using BlackJack.Interfaces;

namespace BlackJack.Implementation
{
    /// <summary>
    /// Handles user input from the console.
    /// Allows for easy testing and potential UI replacement.
    /// </summary>
    public class ConsoleUserInputHandler : IUserInputHandler
    {
        /// <summary>
        /// Reads a single character representing a player action.
        /// </summary>
        /// <returns>The character entered by the user.</returns>
        public char ReadAction()
        {
            return Console.ReadLine()![0];
        }

        /// <summary>
        /// Reads a decimal bet amount from the player.
        /// </summary>
        /// <returns>The bet amount entered by the user.</returns>
        public decimal ReadBet()
        {
            Console.Write("Make a bet -> ");
            return decimal.Parse(Console.ReadLine()!);
        }
    }
}
