using BlackJack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public static class Conditions
    {
        private const int MAX_SCORE = 21;

        public static bool IsBlackJack(IHand hand) 
            => hand.GetScore() == MAX_SCORE;

        public static bool IsBusted(IHand hand) 
            => hand.GetScore() > MAX_SCORE;

        public static bool FiveCardsInHand(IHand hand)
            => hand.PairCards.Count == 5 && hand.GetScore() <= MAX_SCORE;

        public static string EvaluateWinner(IPlayer player, IDealer dealer)
        {
            string winMessage = string.Empty;

            if(player.Hand.GetScore() > dealer.Hand.GetScore())
            {
                player.ChangeMoney(player.Bet * 1.5m);
                winMessage = $"You win! {player.Hand.GetScore()} > {dealer.Hand.GetScore()}";
            }
            else if(player.Hand.GetScore() < dealer.Hand.GetScore())
            {
                player.ChangeMoney(-player.Bet);
                winMessage = $"You lost! {player.Hand.GetScore()} < {dealer.Hand.GetScore()}";
            }
            else
            {
                winMessage = "Draw! You have equal scores!";
            }

            return winMessage;
        }

        public static bool CanHit(IHand hand) 
            => hand.GetScore() < MAX_SCORE;
    }
}
