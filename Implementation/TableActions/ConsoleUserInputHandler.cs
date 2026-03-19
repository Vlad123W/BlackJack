using BlackJack.Implementation.Data;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.TableActions
{
    /// <summary>
    /// Handles user input from the console with improved formatting.
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
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return ' ';

            return input[0];
        }

        /// <summary>
        /// Reads a decimal bet amount from the player with validation.
        /// </summary>
        /// <param name="playerMoney">The player's current balance to validate against.</param>
        /// <returns>The bet amount entered by the user.</returns>
        public decimal ReadBet(decimal playerMoney = decimal.MaxValue)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(ConsoleColors.BoldColorText("╔════════════════════════════════════╗", ConsoleColors.BrightYellow));
                Console.Write(ConsoleColors.ColorText("║ Enter your bet amount: ", ConsoleColors.BrightYellow));

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(ConsoleColors.ColorText("╚════════════════════════════════════╝", ConsoleColors.BrightRed));
                    Console.WriteLine(ConsoleColors.ColorText("✗ Invalid bet! Please enter a positive number.", ConsoleColors.BrightRed));
                    continue;
                }

                if (decimal.TryParse(input, out decimal bet) && bet > 0)
                {
                    // Check if bet exceeds player's balance
                    if (bet > playerMoney)
                    {
                        Console.WriteLine(ConsoleColors.ColorText("╚════════════════════════════════════╝", ConsoleColors.BrightRed));
                        Console.WriteLine(ConsoleColors.ColorText($"✗ Invalid bet! You only have ${playerMoney}. Please enter a smaller amount.", ConsoleColors.BrightRed));
                        continue;
                    }

                    Console.WriteLine(ConsoleColors.ColorText("╚════════════════════════════════════╝", ConsoleColors.BrightYellow));
                    return bet;
                }

                Console.WriteLine(ConsoleColors.ColorText("╚════════════════════════════════════╝", ConsoleColors.BrightRed));
                Console.WriteLine(ConsoleColors.ColorText("✗ Invalid bet! Please enter a positive number.", ConsoleColors.BrightRed));
            }
        }
    }
}
