namespace MtgElo.Core
{
    public class Matchup
    {
        public Deck DeckA { get; set; }

        public Deck DeckB { get; set; }

        public MatchupOutcome Outcome { get; set; }

        public enum MatchupOutcome
        {
            DeckAWon,
            DeckBWon,
            Draw
        }
    }
}
