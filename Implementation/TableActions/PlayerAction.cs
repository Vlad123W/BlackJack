namespace BlackJack.Implementation.TableActions
{
    /// <summary>
    /// Enumeration of possible player actions in the game.
    /// </summary>
    public enum PlayerAction
    {
        /// <summary>
        /// Player requests another card.
        /// </summary>
        Hit = '1',

        /// <summary>
        /// Player ends their turn.
        /// </summary>
        Stand = '2',

        /// <summary>
        /// Player doubles their bet and takes one more card.
        /// </summary>
        Double = '3',

        /// <summary>
        /// Player splits a pair into two hands.
        /// </summary>
        Split = '4',

        /// <summary>
        /// Player exits the game.
        /// </summary>
        Exit = '0'
    }
}
