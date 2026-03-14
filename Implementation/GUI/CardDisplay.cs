using BlackJack.Implementation.Data;
using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.GUI
{
    /// <summary>
    /// Handles beautiful ASCII representation of cards.
    /// </summary>
    public class CardDisplay
    {
        private const int CardWidth = 9;
        private const int CardHeight = 7;

        /// <summary>
        /// Renders a card in ASCII art format.
        /// </summary>
        public static void DisplayCard(Card card)
        {
            if (card.IsHidden)
            {
                DisplayHiddenCard();
                return;
            }

            string color = GetCardColor(card);
            string rank = card.Title[0].ToString();
            string suit = GetSuitSymbol(card.Title);

            Console.WriteLine($"{color}┌─────────┐{ConsoleColors.Reset}");
            Console.WriteLine($"{color}│{rank}       │{ConsoleColors.Reset}");
            Console.WriteLine($"{color}│         │{ConsoleColors.Reset}");
            Console.WriteLine($"{color}│    {suit}    │{ConsoleColors.Reset}");
            Console.WriteLine($"{color}│         │{ConsoleColors.Reset}");
            Console.WriteLine($"{color}│       {rank}│{ConsoleColors.Reset}");
            Console.WriteLine($"{color}└─────────┘{ConsoleColors.Reset}");
        }

        /// <summary>
        /// Displays a single line of cards.
        /// </summary>
        public static void DisplayHandLine(IHand hand, int lineIndex)
        {
            string line = "";

            foreach (var card in hand.PairCards)
            {
                line += GetCardLine(card, lineIndex);
            }

            Console.WriteLine(line);
        }

        private static void DisplayHiddenCard()
        {
            Console.WriteLine($"{ConsoleColors.BrightYellow}┌─────────┐{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}└─────────┘{ConsoleColors.Reset}");
        }

        private static string GetCardLine(Card card, int lineIndex)
        {
            if (card.IsHidden)
                return GetHiddenCardLine(lineIndex) + "  ";

            string color = GetCardColor(card);
            string rank = card.Title[0].ToString();
            string suit = GetSuitSymbol(card.Title);

            return lineIndex switch
            {
                0 => $"{color}┌─────────┐{ConsoleColors.Reset}  ",
                1 => $"{color}│{rank}       │{ConsoleColors.Reset}  ",
                2 => $"{color}│         │{ConsoleColors.Reset}  ",
                3 => $"{color}│    {suit}   │{ConsoleColors.Reset}  ",
                4 => $"{color}│         │{ConsoleColors.Reset}  ",
                5 => $"{color}│       {rank}│{ConsoleColors.Reset}  ",
                6 => $"{color}└─────────┘{ConsoleColors.Reset}  ",
                _ => ""
            };
        }

        private static string GetHiddenCardLine(int lineIndex)
        {
            return lineIndex switch
            {
                0 => $"{ConsoleColors.BrightYellow}┌─────────┐{ConsoleColors.Reset}",
                1 => $"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}",
                2 => $"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}",
                3 => $"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}",
                4 => $"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}",
                5 => $"{ConsoleColors.BrightYellow}│░░░░░░░░░│{ConsoleColors.Reset}",
                6 => $"{ConsoleColors.BrightYellow}└─────────┘{ConsoleColors.Reset}",
                _ => ""
            };
        }

        private static string GetCardColor(Card card)
        {
            if (card.Title.Contains('♥') || card.Title.Contains('♦'))
                return ConsoleColors.BrightRed;

            return ConsoleColors.BrightBlue;
        }

        private static string GetSuitSymbol(string title)
        {
            if (title.Contains('A'))
                return "A";
            if (title.Contains('K'))
                return "K";
            if (title.Contains('Q'))
                return "Q";
            if (title.Contains('J'))
                return "J";

            return title[0].ToString();
        }
    }
}
