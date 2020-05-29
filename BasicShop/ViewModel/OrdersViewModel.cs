using BasicShop.Managers;
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
    public class OrdersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<OrderModel> _orders;
        private Visibility _loadingScreen;

        public Visibility LoadingScreen
        {
            get { return _loadingScreen; }
            set
            {
                if (value == _loadingScreen) return;

                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
        }
        public ObservableCollection<OrderModel> Orders
        {
            get { return _orders; }
            set
            {
                if (value == _orders) return;

                _orders = value;
                OnPropertyChanged("Orders");
            }

        }

        public OrdersViewModel()
        {
            LoadingScreenProcess(() => 
            {
                Orders = new ObservableCollection<OrderModel>(GetOrders());
            });
        }

        private List<OrderModel> GetOrders()
        {
            List<OrderModel> output = new List<OrderModel>();

            List<order> ord = new List<order>();
            try
            {
                var dataContext = new shopEntities();
                ord = dataContext.order.Where(x => x.account_id == AccountManager.LoggedId).ToList();
            }
            catch(Exception e)
            {
                string mess = "Podczas uzyskiwania listy zamówień wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            foreach(var o in ord)
            {
                var tmp = new OrderModel()
                {
                    OrderId = o.order_id,
                    OrderStatus = o.status,
                    Address = o.address,
                };
                foreach(var pr in o.order_product)
                    tmp.Products.Add(new Tuple<product, int>(pr.product, pr.quantity));

                output.Add(tmp);
            }

            return output;
        }
        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
