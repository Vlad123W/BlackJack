using System.Collections.ObjectModel;

namespace BlackJack.Interfaces
{
    /// <summary>
    /// Defines the contract for player actions during the game.
    /// </summary>
    public interface IActions
    {
        /// <summary>
        /// Delegate for game events.
        /// </summary>
        delegate void Notify();

        /// <summary>
        /// Event raised when the player hits and receives a card.
        /// </summary>
        event Notify? Hitted;

        /// <summary>
        /// Event raised when the game ends (stand or bust).
        /// </summary>
        event Notify? GameEnded;

        /// <summary>
        /// Player requests another card.
        /// </summary>
        /// <returns>True if the round should end, false otherwise.</returns>
        bool Hit();

        /// <summary>
        /// Player ends their turn.
        /// </summary>
        /// <returns>True if the round should end.</returns>
        bool Stand();

        /// <summary>
        /// Player doubles their bet and takes one more card.
        /// </summary>
        /// <returns>True if the round should end.</returns>
        bool Double();

        /// <summary>
        /// Player splits a pair into two separate hands.
        /// </summary>
        /// <param name="splitHands">Stack to store the split hands.</param>
        void Split(Stack<IPlayer> splitHands);
    }
}