using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.View
{
    public class SampleViewModel : INotifyPropertyChanged
    {
        private bool _isChecked;
        private string _code;
        private string _name;
        private string _description;

        public bool IsChecked 
        { 
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;

                _isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }
        public string Code 
        {
            get { return _code; }
            set
            {
                if (value == _code) return;

                _code = value;
                this.OnPropertyChanged("Code");
            }
        }
        public string Name 
        {
            get { return _name; }
            set
            {
                if (value == _name) return;

                _name = value;
                this.OnPropertyChanged("Name");
            }
        }
        public string Description 
        {
            get { return _description; }
            set
            {
                if (value == _description) return;

                _description = value;
                this.OnPropertyChanged("Description");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }

    public class SampleVM : INotifyPropertyChanged
    {
        private SampleViewModel _svm;
        public SampleViewModel SVM
        {
            get { return _svm; }
            set
            {
                if (value == _svm)
                    return;

                _svm = value;
                OnPropertyChanged("SVM");
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
