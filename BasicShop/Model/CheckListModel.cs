using System;
using System.ComponentModel;

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

        public CheckListModel()
        {
            ChangeSelection = new Commands.SimpleCommand(SelectionChange);
        }

        private void SelectionChange()
        {
            IsChecked = !IsChecked;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
