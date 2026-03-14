namespace BlackJack.Implementation
{
    /// <summary>
    /// Constants used throughout the game logic.
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// Minimum score at which dealer must stand (17 or higher).
        /// </summary>
        public const int DealerStandScore = 17;

        /// <summary>
        /// Blackjack payout multiplier (1.5x the bet).
        /// </summary>
        public const decimal BlackjackPayout = 1.5m;

        /// <summary>
        /// Double down multiplier (2x the bet).
        /// </summary>
        public const int DoubleMultiplier = 2;

        /// <summary>
        /// Maximum number of cards allowed in a hand (for safety).
        /// </summary>
        public const int MaxCardsInHand = 5;

        /// <summary>
        /// Minimum number of cards needed to start a game.
        /// </summary>
        public const int MinCardsToPlay = 4;

        /// <summary>
        /// Initial number of cards dealt to each player/dealer.
        /// </summary>
        public const int InitialCardsDealt = 2;

        /// <summary>
        /// Starting index for player's first card in deck.
        /// </summary>
        public const int PlayerFirstCardIndex = 0;

        /// <summary>
        /// Starting index for player's second card in deck.
        /// </summary>
        public const int PlayerSecondCardIndex = 1;

        /// <summary>
        /// Starting index for dealer's first card in deck.
        /// </summary>
        public const int DealerFirstCardIndex = 2;

        /// <summary>
        /// Starting index for dealer's second card in deck.
        /// </summary>
        public const int DealerSecondCardIndex = 3;
    }
}
