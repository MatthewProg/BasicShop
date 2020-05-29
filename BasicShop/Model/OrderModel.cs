using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class OrderModel : INotifyPropertyChanged
    {
        private int _orderId;
        private string _orderStatus;
        private List<Tuple<product,int>> _products;
        private address _address;

        public int OrderId
        {
            get { return _orderId; }
            set
            {
                if (value == _orderId) return;

                _orderId = value;
                OnPropertyChanged("OrderId");
            }
        }
        public string OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                if (value == _orderStatus) return;

                _orderStatus = value;
                OnPropertyChanged("OrderStatus");
            }
        }
        public List<Tuple<product, int>> Products
        {
            get { return _products; }
            set
            {
                if (value == _products) return;

                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public address Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;

                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public OrderModel()
        {
            Products = new List<Tuple<product, int>>();
            Address = new address();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
