using BlackJack.Interfaces;

namespace BlackJack.Implementation
{
    /// <summary>
    /// Displays the game state and menu to the player.
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
        /// Displays the current game state to the console.
        /// </summary>
        public void Print()
        {
            PrintTableHeader();
            PrintPlayerInfo();
            PrintDealerInfo();
            PrintGameOutcome();
            PrintFooter();
        }

        private void PrintTableHeader()
        {
            Console.WriteLine("\n=========TABLE==========\n");
        }

        private void PrintPlayerInfo()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Your score: {_player.Hand.GetScore()}");
            _player.Hand.Show();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
        }

        private void PrintDealerInfo()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Dealer:");
            _dealer.Hand.Show();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
        }

        private void PrintGameOutcome()
        {
            Console.WriteLine(WinMessage);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        private void PrintFooter()
        {
            Console.WriteLine($"Money: {_player.Money}");
            Console.WriteLine(MenuString);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
        }

        private void HideDealerSecondCardIfNeeded(bool shouldHide)
        {
            if (_dealer.Hand?.PairCards is not null && _dealer.Hand.PairCards.Count > 1)
            {
                _dealer.Hand.PairCards[1].IsHidden = shouldHide;
            }
        }
    }
}
