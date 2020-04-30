using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class CheckListModel : INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _name;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;

                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Commands.SimpleCommand ChangeSelection { get; set; }
        private void SelectionChange()
        {
            IsChecked = !IsChecked;
        }

        public CheckListModel()
        {
            ChangeSelection = new Commands.SimpleCommand(SelectionChange);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
