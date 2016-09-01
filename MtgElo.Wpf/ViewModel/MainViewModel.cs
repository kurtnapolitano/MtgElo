using GalaSoft.MvvmLight;

namespace MtgElo.Wpf.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core;

    public class MainViewModel : ViewModelBase
    {
        private readonly List<Deck> _allDecks;

        private DeckTypeFilter _filter = DeckTypeFilter.All;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "DeckMatchups.xlsx");
            var reader = new WorkbookReader(path);

            var matchups = reader.GetAllMatchups();

            var calc = new EloCalculator();

            calc.Calculate(matchups);

            var decklist = new List<Deck>();

            decklist.AddRange(matchups.Select(m => m.DeckA).Distinct());
            decklist.AddRange(matchups.Where(m => !decklist.Contains(m.DeckB)).Select(m => m.DeckB).Distinct());

            _allDecks = decklist;
            Decks = _allDecks;
            Filter = new DeckFilter();
            Filter.FilterChangedEvent += OnFilterChanged;
        }

        public List<Deck> Decks { get; set; }

        public DeckFilter Filter { get; set; }

        private void OnFilterChanged(object sender, DeckFilter.FilterChangedEventArgs args)
        {
            switch (args.CurrentFilter)
            {
                case DeckTypeFilter.All:
                    Decks = _allDecks;
                    break;
                case DeckTypeFilter.Commander:
                    Decks = _allDecks.Where(d => d.Type == Deck.DeckType.Commander).ToList();
                    break;
                case DeckTypeFilter.Constructed:
                    Decks = _allDecks.Where(d => d.Type == Deck.DeckType.Constructed).ToList();
                    break;
                default:
                    throw new Exception($"Unrecognized filter: {_filter}");
            }

            RaisePropertyChanged("Decks");
        }
    }
}