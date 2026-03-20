using BlackJack.Implementation.Data;
using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.GUI
{
    /// <summary>
    /// Displays the game state and menu to the player with modern, colorful UI.
    /// Responsible for all console output presentation.
    /// </summary>
    public class GraphicInterface : IGraphicInterface
    {
        private readonly IPlayer _player;
        private readonly IDealer _dealer;
        private readonly MenuBuilder _menuBuilder;

        private bool _canSplit;
        private bool _canDouble;

        /// <summary>
        /// Gets the dynamically built menu string based on available actions.
        /// </summary>
        public string MenuString => _menuBuilder.BuildMenu(_canDouble, _canSplit);

        /// <summary>
        /// Gets or sets the message to display about game outcome.
        /// </summary>
        public string WinMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the player can split their hand.
        /// </summary>
        public bool IsSplitNeeded
        {
            get => _canSplit;
            set => _canSplit = value;
        }

        /// <summary>
        /// Gets or sets whether the player can double down.
        /// </summary>
        public bool IsDoubleNeeded
        {
            get => _canDouble;
            set => _canDouble = value;
        }

        /// <summary>
        /// Initializes a new instance of the GraphicInterface class.
        /// </summary>
        /// <param name="player">The player's game state.</param>
        /// <param name="dealer">The dealer's game state.</param>
        /// <param name="hideDealerSecond">Whether to hide the dealer's second card.</param>
        /// <exception cref="ArgumentNullException">Thrown when player or dealer is null.</exception>
        public GraphicInterface(IPlayer player, IDealer dealer, bool hideDealerSecond = true)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            _menuBuilder = new MenuBuilder();

            HideDealerSecondCardIfNeeded(hideDealerSecond);
        }

        /// <summary>
        /// Displays the current game state to the console with modern styling.
        /// </summary>
        public void Print()
        {
            ClearScreen();
            PrintGameHeader();
            PrintDealerSection();
            PrintPlayerSection();
            PrintGameOutcome();

            if (string.IsNullOrEmpty(WinMessage))
                PrintMenu();

            PrintFooter();
        }

        private static void ClearScreen()
        {
            Console.Clear();
        }

        private static void PrintGameHeader()
        {
            Console.WriteLine(ConsoleColors.BoldColorText("╔═════════════════════════════════════╗", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("║  ♠️  BLACKJACK GAME  ♠️             ║", ConsoleColors.BrightBlue));
            Console.WriteLine(ConsoleColors.BoldColorText("╚═════════════════════════════════════╝", ConsoleColors.BrightBlue));
            Console.WriteLine();
        }

        private void PrintDealerSection()
        {


            Console.WriteLine(ConsoleColors.BoldColorText("┌─ DEALER ────────────────────────────┐", ConsoleColors.Cyan));

            DisplayCardsInRows(_dealer.Hand);

            // If dealer's second card is hidden, show score without that card; otherwise show full score
            int dealerScore;
            if (_dealer.Hand.PairCards.Count > GameConstants.DealerSecondCardIndex && _dealer.Hand.PairCards[GameConstants.DealerSecondCardIndex].IsHidden)
            {
                dealerScore = _dealer.Hand.GetScore() - _dealer.Hand.PairCards[GameConstants.DealerSecondCardIndex].Cost;
            }
            else
            {
                dealerScore = _dealer.Hand.GetScore();
            }

            Console.WriteLine(ConsoleColors.BoldColorText($"├─────────────────────────────────────┤", ConsoleColors.Cyan));
            Console.WriteLine($"{ConsoleColors.Cyan}│ Score: {ConsoleColors.BoldColorText(dealerScore.ToString(), ConsoleColors.BrightYellow),-42}{ConsoleColors.Cyan}│{ConsoleColors.Reset}");
            Console.WriteLine(ConsoleColors.BoldColorText("└─────────────────────────────────────┘", ConsoleColors.Cyan));
            Console.WriteLine();
        }

        private void PrintPlayerSection()
        {
            Console.WriteLine(ConsoleColors.BoldColorText("┌─ YOUR HAND ─────────────────────────┐", ConsoleColors.BrightGreen));

            DisplayCardsInRows(_player.Hand);

            int playerScore = _player.Hand.GetScore();
            string scoreColor = playerScore > 21 ? ConsoleColors.BrightRed : ConsoleColors.BrightGreen;
            string playerScoreString = ConsoleColors.BoldColorText(playerScore.ToString(), scoreColor);

            Console.WriteLine(ConsoleColors.BoldColorText($"├─────────────────────────────────────┤", ConsoleColors.BrightGreen));
            Console.WriteLine($"{ConsoleColors.BrightGreen}│ Score: {ConsoleColors.BoldColorText(playerScore.ToString(), scoreColor),-42}{ConsoleColors.BrightGreen}│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightGreen}│ Bet: {ConsoleColors.BoldColorText($"${_player.Bet}", ConsoleColors.BrightYellow),-44}{ConsoleColors.BrightGreen}│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightGreen}│ Balance: {ConsoleColors.BoldColorText($"${_player.Money}", ConsoleColors.BrightYellow),-40}{ConsoleColors.BrightGreen}│{ConsoleColors.Reset}");
            Console.WriteLine(ConsoleColors.BoldColorText("└─────────────────────────────────────┘", ConsoleColors.BrightGreen));
            Console.WriteLine();
        }

        private void PrintGameOutcome()
        {
            if (!string.IsNullOrEmpty(WinMessage))
            {
                Console.WriteLine();

                if (WinMessage.Contains("win", System.StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(ConsoleColors.BoldColorText("╔═════════════════════════════════════╗", ConsoleColors.BrightGreen));
                    Console.WriteLine(ConsoleColors.BoldColorText($"║ {WinMessage.Trim(),-35} ║", ConsoleColors.BrightGreen));
                    Console.WriteLine(ConsoleColors.BoldColorText("╚═════════════════════════════════════╝", ConsoleColors.BrightGreen));
                }
                else if (WinMessage.Contains("lost", System.StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(ConsoleColors.BoldColorText("╔═════════════════════════════════════╗", ConsoleColors.BrightRed));
                    Console.WriteLine(ConsoleColors.BoldColorText($"║ {WinMessage.Trim(),-35} ║", ConsoleColors.BrightRed));
                    Console.WriteLine(ConsoleColors.BoldColorText("╚═════════════════════════════════════╝", ConsoleColors.BrightRed));
                }
                else if (WinMessage.Contains("Tie", System.StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(ConsoleColors.BoldColorText("╔═════════════════════════════════════╗", ConsoleColors.BrightYellow));
                    Console.WriteLine(ConsoleColors.BoldColorText($"║ {WinMessage.Trim(),-35} ║", ConsoleColors.BrightYellow));
                    Console.WriteLine(ConsoleColors.BoldColorText("╚═════════════════════════════════════╝", ConsoleColors.BrightYellow));
                }

                Console.WriteLine();
            }
        }

        private void PrintMenu()
        {

            string menu = MenuString;

            int totalWidth = menu.Length + 5;

            int leftPadding = (totalWidth - menu.Length) / 2;
            int rightPadding = totalWidth - menu.Length - leftPadding;

            Console.WriteLine(ConsoleColors.BoldColorText($"┌─ ACTIONS{new string('─', menu.Length - 4)}┐", ConsoleColors.BrightYellow));
            Console.WriteLine($"{ConsoleColors.BrightYellow}│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│{new string(' ', leftPadding)}{ConsoleColors.BoldColorText(menu, ConsoleColors.Yellow)}{new string(' ', rightPadding)}{ConsoleColors.BrightYellow}│{ConsoleColors.Reset}");
            Console.WriteLine($"{ConsoleColors.BrightYellow}│{ConsoleColors.Reset}");
            Console.WriteLine(ConsoleColors.BoldColorText($"└─────────{new string('─', menu.Length - 4)}┘", ConsoleColors.BrightYellow));
        }

        private static void PrintFooter()
        {
            Console.WriteLine();
            Console.Write($"{ConsoleColors.Dim}Enter your choice: {ConsoleColors.Reset}");
        }

        private static void DisplayCardsInRows(IHand hand)
        {
            const int cardHeight = 7;

            for (int line = 0; line < cardHeight; line++)
            {
                Console.Write($"{ConsoleColors.Cyan}│{ConsoleColors.Reset} ");

                foreach (var card in hand.PairCards)
                {
                    if (card.IsHidden)
                    {
                        PrintHiddenCardLine(line);
                    }
                    else
                    {
                        PrintCardLine(card, line);
                    }
                    Console.Write(" ");
                }

                Console.WriteLine($"{ConsoleColors.Cyan}│{ConsoleColors.Reset}");
            }
        }

        private static void PrintCardLine(Card card, int lineIndex)
        {
            string color = GetCardColor(card);
            string rank = card.Title.Length == 3 ? card.Title[..2] : card.Title[0].ToString();
            string suit = GetSuitSymbol(card.Title);

            string line = lineIndex switch
            {
                0 => $"{color}┌─────┐{ConsoleColors.Reset}",
                1 => rank == "10" ? $"{color}│{rank}   │{ConsoleColors.Reset}" : $"{color}│{rank}    │{ConsoleColors.Reset}",
                2 => $"{color}│     │{ConsoleColors.Reset}",
                3 => $"{color}│  {suit}  │{ConsoleColors.Reset}",
                4 => $"{color}│     │{ConsoleColors.Reset}",
                5 => rank == "10" ? $"{color}│   {rank}│{ConsoleColors.Reset}" : $"{color}│   {rank} │{ConsoleColors.Reset}",
                6 => $"{color}└─────┘{ConsoleColors.Reset}",
                _ => ""
            };

            Console.Write(line);
        }

        private static void PrintHiddenCardLine(int lineIndex)
        {
            string line = lineIndex switch
            {
                0 => $"{ConsoleColors.BrightYellow}┌─────┐{ConsoleColors.Reset}",
                1 => $"{ConsoleColors.BrightYellow}│░░░░░│{ConsoleColors.Reset}",
                2 => $"{ConsoleColors.BrightYellow}│░░░░░│{ConsoleColors.Reset}",
                3 => $"{ConsoleColors.BrightYellow}│░░░░░│{ConsoleColors.Reset}",
                4 => $"{ConsoleColors.BrightYellow}│░░░░░│{ConsoleColors.Reset}",
                5 => $"{ConsoleColors.BrightYellow}│░░░░░│{ConsoleColors.Reset}",
                6 => $"{ConsoleColors.BrightYellow}└─────┘{ConsoleColors.Reset}",
                _ => ""
            };

            Console.Write(line);
        }

        private static string GetCardColor(Card card)
        {
            // Каждой масти свой цвет для лучшей различимости
            if (card.Title.Contains('♥'))
                return ConsoleColors.BrightRed;           // Сердца - красный

            if (card.Title.Contains('♦'))
                return ConsoleColors.BrightMagenta;       // Бубны - магента

            if (card.Title.Contains('♣'))
                return ConsoleColors.BrightGreen;         // Трефы - зелёный

            if (card.Title.Contains('♠'))
                return ConsoleColors.BrightBlue;          // Пики - синий

            return ConsoleColors.White;
        }

        private static string GetSuitSymbol(string title)
        {
            if (title.Contains('♥'))
                return "♥";
            if (title.Contains('♦'))
                return "♦";
            if (title.Contains('♣'))
                return "♣";
            if (title.Contains('♠'))
                return "♠";

            return "●";
        }

        private void HideDealerSecondCardIfNeeded(bool shouldHide)
        {
            if (_dealer.Hand?.PairCards is not null && _dealer.Hand.PairCards.Count > GameConstants.DealerSecondCardIndex)
            {
                _dealer.Hand.PairCards[GameConstants.DealerSecondCardIndex].IsHidden = shouldHide;
            }
        }
    }
}
