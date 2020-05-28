using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;

        private ObservableCollection<CartModel> _cart;
        private Visibility _loadingScreen;

        public ObservableCollection<CartModel> Cart
        {
            get { return _cart; }
            set
            {
                if (value == _cart) return;

                _cart = value;
                OnPropertyChanged("Cart");
            }
        }
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
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public SimpleCommand OrderProcessCommand { get; set; }
        public ParameterCommand RemoveElementCommand { get; set; }

        public CartViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue();

            OrderProcessCommand = new SimpleCommand(ProcessOrder);
            RemoveElementCommand = new ParameterCommand(RemoveElement);

            LoadingScreenProcess(() =>
            {
                Cart = new ObservableCollection<CartModel>(GetCartList());
            });
        }

        private void ProcessOrder()
        {
            if (Cart.Count == 0)
            {
                MessageQueue.IgnoreDuplicate = true;
                MessageQueue.Enqueue("Koszyk jest pusty!", null, null, null, false, false, new TimeSpan(0, 0, 4));
                return;
            }
            if (AccountManager.LoggedId == null)
            {
                MessageQueue.IgnoreDuplicate = true;
                MessageQueue.Enqueue("Musisz być zalogowany, aby złożyć zamówienie", null, null, null, false, false, new TimeSpan(0, 0, 4));
                return;
            }

            foreach(var element in Cart)
                _mainVM.Cart[element.ProductId] = element.Quantity;

            _mainVM.LoadPage("checkout");
        }
        private List<CartModel> GetCartList()
        {
            List<CartModel> output = new List<CartModel>();

            try
            {
                var dataContext = new shopEntities();
                foreach(var key in _mainVM.Cart.Keys)
                {
                    CartModel prod = new CartModel();
                    prod.ProductId = key;
                    prod.ProductName = dataContext.product.FirstOrDefault(x => x.product_id == key).name;
                    prod.Quantity = _mainVM.Cart[key];
                    output.Add(prod);
                }
            }
            catch(Exception e)
            {
                string mess = "Podczas uzyskiwania listy produktów z koszyka wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void RemoveElement(object param)
        {
            var element = param as CartModel;
            Cart.Remove(element);
            _mainVM.Cart.Remove(element.ProductId);
            _mainVM.UpdateCart();
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
