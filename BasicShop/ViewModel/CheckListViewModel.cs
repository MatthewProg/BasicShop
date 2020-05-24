using BasicShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class CheckListViewModel : INotifyPropertyChanged, Interfaces.IFilter
    {
        private ObservableCollection<CheckListModel> _checks;
        private string _header;

        public ObservableCollection<CheckListModel> Checks
        {
            get { return _checks; }
            set
            {
                if (value == _checks) return;

                _checks = value;
                OnPropertyChanged("Checks");
            }
        }
        
        public string Header
        {
            get { return _header; }
            set
            {
                if (value == _header) return;

                _header = value;
                OnPropertyChanged("Header");
            }
        }

        public int NoOfChecked
        {
            get
            {
                int count = 0;
                foreach (var e in _checks)
                    if (e.IsChecked) count++;
                return count;
            }
        }

        public CheckListViewModel()
        {
            Checks = new ObservableCollection<CheckListModel>();
            Header = "Header";
        }

        public CheckListViewModel(ObservableCollection<CheckListModel> checks, string header = "Header") : base()
        {
            Checks = checks;
            Header = header;
        }

        public CheckListViewModel(List<CheckListModel> checks, string header = "Header") : base()
        {
            Checks = new ObservableCollection<CheckListModel>(checks);
            Header = header;
        }

        public List<SpecifitationModel> GetActiveFilters()
        {
            var output = new List<SpecifitationModel>();

            foreach (var e in _checks)
                if (e.IsChecked) output.Add(new SpecifitationModel() { Element = Header, Value = e.Name });

            return output;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
