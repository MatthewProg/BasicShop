using System;
using System.ComponentModel;

namespace BasicShop.ViewModel
{
    public class SliderListViewModel : INotifyPropertyChanged, Interfaces.IFilter
    {
        private float _minimum;
        private float _maximum;
        private float _valueMinimum;
        private float _valueMaximum;
        private string _header;
        private float _step;

        public int Precision
        {
            get
            {
                for (int i = 0; i < 4; i++)
                {
                    if ((float)(Step * (float)Math.Pow(10, i)) >= 1.0F) return i;
                }
                return 4;
            }
        }
        public float Step
        {
            get { return _step; }
            set
            {
                if (value == _step) return;

                _step = value;
                OnPropertyChanged("Step");
                OnPropertyChanged("Precision");
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
        public float Minimum
        {
            get { return _minimum; }
            set
            {
                if (value == _minimum) return;

                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }
        public float Maximum
        {
            get { return _maximum; }
            set
            {
                if (value == _maximum) return;

                _maximum = value;
                OnPropertyChanged("Maximum");
            }
        }
        public float ValueMinimum
        {
            get { return (float)Math.Round(_valueMinimum, Precision); }
            set
            {
                if (value == _valueMinimum) return;

                _valueMinimum = value;
                OnPropertyChanged("ValueMinimum");
            }
        }

        public float ValueMaximum
        {
            get { return (float)Math.Round(_valueMaximum, Precision); }
            set
            {
                if (value == _valueMaximum) return;

                _valueMaximum = value;
                OnPropertyChanged("ValueMaximum");
            }
        }

        public SliderListViewModel(string header = "Header")
        {
            Minimum = 0.0F;
            Maximum = 1.0F;
            ValueMinimum = Minimum;
            ValueMaximum = Maximum;
            Step = 1;
            Header = header;
        }

        public SliderListViewModel(float min, float max, float step, string header = "Header")
        {
            Minimum = min;
            Maximum = max;
            ValueMinimum = Minimum;
            ValueMaximum = Maximum;
            Step = step;
            Header = header;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
