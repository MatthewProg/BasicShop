using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class ComboBoxItemModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        private object _value;

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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;

                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;

                _value = value;
                OnPropertyChanged("Value");
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
