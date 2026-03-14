using BlackJack.Interfaces;

namespace BlackJack.Implementation.GUI
{
    /// <summary>
    /// Responsible for building menu strings based on available game actions.
    /// Follows the Single Responsibility Principle.
    /// </summary>
    public class MenuBuilder
    {
        private const string BaseMenu = "[1] Hit  [2] Stand  [0] Exit";

        /// <summary>
        /// Builds the menu string based on available actions.
        /// </summary>
        /// <param name="canDouble">Whether the player can double down.</param>
        /// <param name="canSplit">Whether the player can split.</param>
        /// <returns>Formatted menu string with available actions.</returns>
        public string BuildMenu(bool canDouble, bool canSplit)
        {
            if (!canDouble && !canSplit)
                return BaseMenu;

            if (canDouble && !canSplit)
                return "[1] Hit  [2] Stand  [3] Double  [0] Exit";

            if (!canDouble && canSplit)
                return "[1] Hit  [2] Stand  [4] Split  [0] Exit";

            return "[1] Hit  [2] Stand  [3] Double  [4] Split  [0] Exit";
        }
    }
}
