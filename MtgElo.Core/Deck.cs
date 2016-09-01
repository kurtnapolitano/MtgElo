namespace MtgElo.Core
{
    public class Deck
    {
        public Deck(string name, string owner, DeckType type)
        {
            Name = name;
            Owner = owner;
            Type = type;
            Rating = 1200;
        }

        public string Name { get; }

        public string Owner { get; }

        public int Rating { get; internal set; }

        public DeckType Type { get; }

        public enum DeckType
        {
            Constructed,
            Commander
        }
    }
}
