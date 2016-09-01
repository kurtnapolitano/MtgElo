namespace MtgElo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.Linq;

    public class WorkbookReader
    {
        private readonly string _connectionString;

        public WorkbookReader(string workbookPath)
        {
            _connectionString =
                $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={workbookPath};Extended Properties=Excel 12.0;";
        }

        public List<Matchup> GetAllMatchups()
        {
            var allDecks = GetAllDecks();

            var adapter = new OleDbDataAdapter("SELECT * FROM [Matchups$]", _connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "Matchups");

            var data = ds.Tables["Matchups"].AsEnumerable();

            return data.Where(m => !string.IsNullOrWhiteSpace(m.Field<string>("DeckA")))
                .Select(m => CreateMatchup(allDecks, m)).ToList();
        }

        private List<Deck> GetAllDecks()
        {
            var adapter = new OleDbDataAdapter("SELECT * FROM [Decks$]", _connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "Decks");

            var data = ds.Tables["Decks"].AsEnumerable();

            return data.Where(d => !string.IsNullOrWhiteSpace(d.Field<string>("Deck Name")))
                .Select(d => new Deck(
                    d.Field<string>("Deck Name"),
                    d.Field<string>("Owner"),
                    GetDeckType(d.Field<string>("Type")))).ToList();
        }

        private Deck.DeckType GetDeckType(string deckTypeString)
        {
            switch (deckTypeString)
            {
                case "CST":
                    return Deck.DeckType.Constructed;
                case "EDH":
                    return Deck.DeckType.Commander;
                default:
                    throw new Exception($"Unrecognized deck type: {deckTypeString}");
            }
        }

        private Matchup CreateMatchup(List<Deck> allDecks, DataRow dataRow)
        {
            var deckAName = dataRow.Field<string>("DeckA");
            var deckBName = dataRow.Field<string>("DeckB");
            var deckA = allDecks.FirstOrDefault(d => d.Name == deckAName);
            var deckB = allDecks.FirstOrDefault(d => d.Name == deckBName);
            
            if (deckA == null)
            {
                throw new Exception($"Unrecognized deck: {deckAName}");  
            }

            if (deckB == null)
            {
                throw new Exception($"Unrecognized deck: {deckBName}");
            }

            var outcome = GetOutcome(dataRow.Field<string>("Outcome"));

            return new Matchup
                {
                    DeckA = deckA,
                    DeckB = deckB,
                    Outcome = outcome
                };
        }

        private Matchup.MatchupOutcome GetOutcome(string outcomeString)
        {
            switch (outcomeString)
            {
                case "A":
                    return Matchup.MatchupOutcome.DeckAWon;
                case "B":
                    return Matchup.MatchupOutcome.DeckBWon;
                case "D":
                    return Matchup.MatchupOutcome.Draw;
                default:
                    throw new Exception($"Unrecognized outcome: {outcomeString}");
            }
        }
    }
}
