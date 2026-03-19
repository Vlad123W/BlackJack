using BlackJack.Implementation.Entities;
using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return hand.PairCards.Count == 2 ? ProcessTwoAces(hand) : ProcessOneAce(hand);
        }

        private static bool ProcessTwoAces(IHand hand)
        {
            var pairCards = hand.PairCards;

            if (!pairCards.All(x => x.Title.StartsWith('A'))) return false;
         
            pairCards.TryGetValue(x => x.Cost != 1, out Card value);

            if(value != null)
                value.Cost = 1;

            return true;
        }

        private static bool ProcessOneAce(IHand hand)
        {
            if (hand.PairCards.TryGetValue(x => x.Title.StartsWith('A'), out Card value))
            {

                return true;
            }

            return false;
        }

        private static bool TryGetValue(this IList<Card> cards, Predicate<Card> selector, out Card? value)
        {
            foreach (var item in cards)
            {
                if(selector(item))
                {
                    value = item;

                    return true;
                }
            }

            value = default;

            return false;
        }
    }
}
