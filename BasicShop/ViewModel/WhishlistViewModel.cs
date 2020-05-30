using BasicShop.Commands;
using BasicShop.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class WhishlistViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;
        private ObservableCollection<product> _whishlist;
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
        public ObservableCollection<product> Whishlist
        {
            get { return _whishlist; }
            set
            {
                if (value == _whishlist) return;

                _whishlist = value;
                OnPropertyChanged("Whishlist");
            }
        }

        public ParameterCommand RemoveElementCommand { get; set; }
        public ParameterCommand ShowProductCommand { get; set; }

        public WhishlistViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;

            Whishlist = new ObservableCollection<product>();

            LoadingScreen = Visibility.Collapsed;
            RemoveElementCommand = new ParameterCommand(RemoveFromWhishlist);
            ShowProductCommand = new ParameterCommand(ShowProduct);

            Refresh();
        }

        public List<product> GetWhishlistProducts()
        {
            List<product> output = new List<product>();

            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                var a = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                output = a.product.ToList();
            }
            catch (Exception e)
            {
                string mess = "Podczas uzyskiwania listy życzeń wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void Refresh()
        {
            LoadingScreenProcess(() =>
            {
                Whishlist = new ObservableCollection<product>(GetWhishlistProducts());
            });
        }
        private void ShowProduct(object param)
        {
            product p = param as product;
            _mainVM.LoadProduct(p.product_id);
        }
        private void RemoveFromWhishlist(object param)
        {
            product p = param as product;

            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                var a = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                var prod = dataContext.product.FirstOrDefault(x => x.product_id == p.product_id);
                a.product.Remove(prod);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Podczas usuwania produktu z listy życzeń wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            Refresh();
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
