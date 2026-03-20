using BlackJack.Interfaces;

namespace BlackJack.Implementation.TableActions
{

    public static class Conditions
    {
        private const string winPrefix = "You win!";
        private const string losePrefix = "You lost!";
        private const string drawPrefix = "Draw!";

        private const int MAX_SCORE = 21;

        public static bool IsBlackJack(IHand hand)
            => hand.GetScore() == MAX_SCORE;

        public static bool IsBusted(IHand hand)
            => hand.GetScore() > MAX_SCORE;

        public static bool FiveCardsInHand(IHand hand)
            => hand.PairCards.Count == 5 && hand.GetScore() <= MAX_SCORE;

        public static string EvaluateWinner(IPlayer player, IDealer dealer)
        {
            if (player.Hand.GetScore() > dealer.Hand.GetScore())
            {
                return $"{winPrefix} {player.Hand.GetScore()} > {dealer.Hand.GetScore()}";
            }
            else if (player.Hand.GetScore() < dealer.Hand.GetScore())
            {
                return $"{losePrefix} {player.Hand.GetScore()} < {dealer.Hand.GetScore()}";
            }
            else
            {
                return $"{drawPrefix} You have equal scores!";
            }
        }

        public static bool CanHit(IHand hand)
            => hand.GetScore() < MAX_SCORE && hand.GetScore() > 0;
    }
}
