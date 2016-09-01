using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtgElo.Wpf.ViewModel
{
    public class DeckFilter
    {
        private DeckTypeFilter _filter = DeckTypeFilter.All;

        private bool _isAll = true;
        private bool _isConstructed = false;
        private bool _isCommander = false;


        public event EventHandler<FilterChangedEventArgs> FilterChangedEvent;

        public bool IsAll
        {
            get { return _isAll; }
            set
            {
                _isAll = value;
                if(value)
                    RaiseFilterChangedEvent(DeckTypeFilter.All);
            }
        }

        public bool IsConstructed
        {
            get { return _isConstructed; }
            set
            {
                _isConstructed = value; 
                if(value)
                    RaiseFilterChangedEvent(DeckTypeFilter.Constructed);
            }
        }

        public bool IsCommander
        {
            get { return _isCommander; }
            set
            {
                _isCommander = value; 
                if(value)
                    RaiseFilterChangedEvent(DeckTypeFilter.Commander);
            }
        }

        private void RaiseFilterChangedEvent(DeckTypeFilter filter)
        {
            var handler = FilterChangedEvent;
            if (handler != null)
            {
                handler(this, new FilterChangedEventArgs(filter));
            }
        }

        public class FilterChangedEventArgs : EventArgs
        {
            public FilterChangedEventArgs(DeckTypeFilter currentFilter)
            {
                CurrentFilter = currentFilter;
            }

            public DeckTypeFilter CurrentFilter { get; }
        }
    }
}
