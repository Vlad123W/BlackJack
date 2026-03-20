using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;

namespace BlackJack.Implementation.TableActions
{
    public static class HandProcessor
    {
        /// <summary>
        /// Desides if busting caused by Ace.
        /// </summary>
        /// <param name="hand">The hand to process. Must not be null and should contain the cards to evaluate.</param>
        /// <returns>true if busting caused by Ace; otherwise, false.</returns>
        public static bool ProcessCards(IHand hand)
        {
            var pairCards = hand.PairCards;

            if (pairCards.All(x => x.Title.StartsWith('A'))
                && pairCards.TryGetValue(x => x.Cost != 1, out Card? value1))
            {
                value1.Cost = 1;
                return true;
            }
            else if (pairCards.Any(x => x.Title.StartsWith('A'))
                    && pairCards.TryGetValue(x => x.Cost != 1, out Card? value2, out int? index)
                    && index == pairCards.Count - 1)
            {
                value2.Cost = 1;
                return true;
            }

            return false;
        }

        private static bool TryGetValue(this IList<Card> cards, Predicate<Card> selector, out Card? value)
        {
            foreach (var item in cards)
            {
                if (selector(item))
                {
                    value = item;

                    return true;
                }
            }

            value = default;

            return false;
        }

        private static bool TryGetValue(this IList<Card> cards, Predicate<Card> selector, out Card? value, out int? index)
        {

            for (int i = 0; i < cards.Count; i++)
            {
                if (selector(cards[i]))
                {
                    value = cards[i];
                    index = i;

                    return true;
                }
            }

            value = default;
            index = default;

            return false;
        }
    }
}
