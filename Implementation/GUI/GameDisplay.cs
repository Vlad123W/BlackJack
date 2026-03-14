using BlackJack.Implementation.Data;

namespace BlackJack.Implementation.GUI
{
    /// <summary>
    /// Displays game welcome screen and introduction messages.
    /// </summary>
    public class GameDisplay
    {
        /// <summary>
        /// Displays the welcome screen at game start.
        /// </summary>
        public static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(ConsoleColors.BoldColorText("╔══════════════════════════════════════════════════════╗", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║                                                      ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║        ♠️  WELCOME TO BLACKJACK GAME  ♠️              ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║                                                      ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("╚══════════════════════════════════════════════════════╝", ConsoleColors.BrightBlue));
            Console.WriteLine();
            Console.WriteLine(ConsoleColors.ColorText("Game Rules:", ConsoleColors.BrightYellow));
            Console.WriteLine(ConsoleColors.ColorText("===========", ConsoleColors.BrightYellow));
            Console.WriteLine($"  • Beat the dealer's hand without going over 21");
            Console.WriteLine($"  • Face cards (K, Q, J) are worth 10 points");
            Console.WriteLine($"  • Aces are worth 1 or 11 points (whichever is better)");
            Console.WriteLine($"  • Blackjack (21 on first two cards) pays 1.5x your bet");
            Console.WriteLine($"  • Bust (over 21) means you lose");
            Console.WriteLine();
            Console.WriteLine(ConsoleColors.ColorText("Available Actions:", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.ColorText("=================", ConsoleColors.BrightBlue));
            Console.WriteLine($"  [1] Hit     - Take another card");
            Console.WriteLine($"  [2] Stand   - End your turn");
            Console.WriteLine($"  [3] Double  - Double your bet and take one more card");
            Console.WriteLine($"  [4] Split   - Split a pair into two hands (when available)");
            Console.WriteLine($"  [0] Exit    - Quit the game");
            Console.WriteLine();
            Console.Write(ConsoleColors.BoldColorText("Press ENTER to start the game...", ConsoleColors.BrightGreen));
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Displays goodbye message when game ends.
        /// </summary>
        public static void DisplayGoodbyeScreen(decimal finalBalance)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(ConsoleColors.BoldColorText("╔══════════════════════════════════════════════════════╗", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║                                                      ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║              THANK YOU FOR PLAYING!                  ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║                                                      ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("╚══════════════════════════════════════════════════════╝", ConsoleColors.BrightBlue));
            Console.WriteLine();
            Console.WriteLine($"Final Balance: {ConsoleColors.BoldColorText($"${finalBalance}", ConsoleColors.BrightYellow)}");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a round separator.
        /// </summary>
        public static void DisplayRoundSeparator(int roundNumber)
        {
            Console.WriteLine();
            Console.WriteLine(ConsoleColors.ColorText($"{'═'} ROUND {roundNumber} {'═'}", ConsoleColors.BrightRed));
            Console.WriteLine();
        }
    }
}
