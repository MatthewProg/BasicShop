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
    public class CheckListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CheckListModel> _checks;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
