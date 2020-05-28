using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class WarehouseModel : INotifyPropertyChanged
    {
        private int _quantity;
        private string _email;
        private string _phone;
        private string _road;
        private string _house;
        private string _flat;
        private string _zipCode;
        private string _city;
        private string _country;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value == _quantity) return;

                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;

                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;

                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string Road
        {
            get { return _road; }
            set
            {
                if (value == _road) return;

                _road = value;
                OnPropertyChanged("Road");
            }
        }
        public string House
        {
            get { return _house; }
            set
            {
                if (value == _house) return;

                _house = value;
                OnPropertyChanged("House");
            }
        }
        public string Flat
        {
            get { return _flat; }
            set
            {
                if (value == _flat) return;

                _flat = value;
                OnPropertyChanged("Flat");
            }
        }
        public string ZipCode
        {
            get { return _zipCode; }
            set
            {
                if (value == _zipCode) return;

                _zipCode = value;
                OnPropertyChanged("ZipCode");
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                if (value == _city) return;

                _city = value;
                OnPropertyChanged("City");
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                if (value == _country) return;

                _country = value;
                OnPropertyChanged("Country");
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
