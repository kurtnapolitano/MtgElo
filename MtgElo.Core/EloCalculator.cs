namespace MtgElo.Core
{
    using System;
    using System.Collections.Generic;

    public class EloCalculator
    {
        private const double K = 32.0;

        public void Calculate(List<Matchup> matchups)
        {
            matchups.ForEach(ProcessMatchup);
        }

        private void ProcessMatchup(Matchup matchup)
        {
            var r1 = Math.Pow(10, matchup.DeckA.Rating / 400.0);
            var r2 = Math.Pow(10, matchup.DeckB.Rating / 400.0);

            var e1 = r1 / (r1 + r2);
            var e2 = r2 / (r1 + r2);

            var s1 = GetS1(matchup.Outcome);
            var s2 = GetS2(matchup.Outcome);

            var rp1 = matchup.DeckA.Rating + K*(s1 - e1);
            var rp2 = matchup.DeckB.Rating + K*(s2 - e2);

            matchup.DeckA.Rating = (int) rp1;
            matchup.DeckB.Rating = (int) rp2;
        }

        private double GetS1(Matchup.MatchupOutcome outcome)
        {
            switch (outcome)
            {
                case Matchup.MatchupOutcome.DeckAWon:
                    return 1;
                case Matchup.MatchupOutcome.DeckBWon:
                    return 0;
                case Matchup.MatchupOutcome.Draw:
                    return 0.5;
                default:
                    throw new Exception($"Unknown outcome: {outcome}");
            }
        }

        private double GetS2(Matchup.MatchupOutcome outcome)
        {
            switch (outcome)
            {
                case Matchup.MatchupOutcome.DeckAWon:
                    return 0;
                case Matchup.MatchupOutcome.DeckBWon:
                    return 1;
                case Matchup.MatchupOutcome.Draw:
                    return 0.5;
                default:
                    throw new Exception($"Unknown outcome: {outcome}");
            }
        }
    }
}
