using System.Collections.ObjectModel;

namespace BlackJack.Interfaces
{
    /// <summary>
    /// Defines the contract for player actions during the game.
    /// </summary>
    public interface IActions
    {
        /// <summary>
        /// Player requests another card.
        /// </summary>
        /// <returns>True if the round should end, false otherwise.</returns>
        ActionResult Hit();

        /// <summary>
        /// Player ends their turn.
        /// </summary>
        /// <returns>True if the round should end.</returns>
        ActionResult Stand();

        /// <summary>
        /// Player doubles their bet and takes one more card.
        /// </summary>
        /// <returns>True if the round should end.</returns>
        ActionResult Double();

        /// <summary>
        /// Player splits a pair into two separate hands.
        /// </summary>
        /// <param name="splitHands">Stack to store the split hands.</param>
        ActionResult Split(Stack<IPlayer> splitHands);
    }
}