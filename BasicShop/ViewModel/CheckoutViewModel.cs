using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class CheckoutViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;

        private Visibility _loadingScreen;
        private ObservableCollection<CartModel> _cart;
        private decimal _productSum;
        private decimal _deliveryCoast;
        private decimal _paymentCoast;
        private string _road;
        private string _city;
        private string _country;
        private string _house;
        private string _flat;
        private string _zipcode;
        private ObservableCollection<string> _countries;


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
        public decimal ProductSum
        {
            get { return _productSum; }
            set
            {
                if (value == _productSum) return;

                _productSum = value;
                OnPropertyChanged("ProductSum");
                OnPropertyChanged("AllSum");
            }
        }
        public decimal DeliveryCoast
        {
            get { return _deliveryCoast; }
            set
            {
                if (value == _deliveryCoast) return;

                _deliveryCoast = value;
                OnPropertyChanged("DeliveryCoast");
                OnPropertyChanged("AllSum");
            }
        }
        public decimal PaymentCoast
        {
            get { return _paymentCoast; }
            set
            {
                if (value == _paymentCoast) return;

                _paymentCoast = value;
                OnPropertyChanged("PaymentCoast");
                OnPropertyChanged("AllSum");
            }
        }
        public decimal AllSum
        {
            get { return _productSum + _deliveryCoast + _paymentCoast; }
        }
        public string DeliverySelected
        {
            set
            {
                DeliveryCoast = ValueParser(value);
            }
        }
        public string PaymentSelected
        {
            set
            {
                PaymentCoast = ValueParser(value);
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
        public string Zipcode
        {
            get { return _zipcode; }
            set
            {
                if (value == _zipcode) return;

                _zipcode = value;
                OnPropertyChanged("Zipcode");
            }
        }
        public ObservableCollection<string> Countries
        {
            get { return _countries; }
            set
            {
                if (value == _countries) return;

                _countries = value;
                OnPropertyChanged("Countries");
            }
        }
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }


        public SimpleCommand ConfirmCommand { get; set; }

        public CheckoutViewModel(MainWindowViewModel mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));
            MessageQueue.IgnoreDuplicate = true;

            ConfirmCommand = new SimpleCommand(Confirm);

            LoadingScreenProcess(() =>
            {
                Cart = new ObservableCollection<CartModel>(GetCartList());
                Countries = new ObservableCollection<string>(GetCountries());
                ProductSum = SumProducts();
                GetUserDefaultData();
            });
        }

        private void Confirm()
        {
            if (string.IsNullOrWhiteSpace(Road))
            {
                MessageQueue.Enqueue("Pole ulica nie może być puste");
                return;
            }
            if (string.IsNullOrWhiteSpace(City))
            {
                MessageQueue.Enqueue("Pole miasto nie może być puste");
                return;
            }
            if (string.IsNullOrWhiteSpace(Country))
            {
                MessageQueue.Enqueue("Pole kraj nie może być puste");
                return;
            }
            if (string.IsNullOrWhiteSpace(House))
            {
                MessageQueue.Enqueue("Pole dom nie może być puste");
                return;
            }
            if (string.IsNullOrWhiteSpace(Zipcode))
            {
                MessageQueue.Enqueue("Pole kod pocztowy nie może być puste");
                return;
            }

            Road = Road.Trim();
            City = City.Trim();
            Country = Country.Trim();
            if (Flat != null)
            {
                Flat = Flat.Trim();
                if (Flat == string.Empty)
                    Flat = null;
            }
            House = House.Trim();
            Zipcode = Zipcode.Trim();

            if (Zipcode.Length != 5)
            {
                MessageQueue.Enqueue("Kod pocztowy musi mieć 5 znaków");
                return;
            }

            order o = new order();
            o.account_id = (int)AccountManager.LoggedId;
            o.status = "Nieopłacone";

            int cityId;
            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());

                var query = dataContext.address.FirstOrDefault(x => x.road == Road && x.house == House && x.flat == Flat && x.zip_code == Zipcode && x.city.city1 == City);

                if (query != null)
                    o.address_id = query.address_id;
                else
                {
                    if (dataContext.city.Count(x => x.city1 == City) != 0)
                        cityId = dataContext.city.FirstOrDefault(x => x.city1 == City).city_id;
                    else
                    {
                        int countryId = dataContext.country.FirstOrDefault(x => x.country1 == Country).country_id;
                        dataContext.city.Add(new city() { city1 = City, country_id = countryId });
                        dataContext.SaveChanges();
                        cityId = dataContext.city.FirstOrDefault(x => x.city1 == City).city_id;
                    }

                    o.address = new address();
                    o.address.road = Road;
                    o.address.city_id = cityId;
                    o.address.flat = Flat;
                    o.address.house = House;
                    o.address.zip_code = Zipcode;
                }
            }
            catch (Exception e)
            {
                string mess = "Podczas zapisywania zamówienia wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
                return;
            }

            foreach (var p in Cart)
                o.order_product.Add(new order_product() { product_id = p.ProductId, quantity = p.Quantity });
            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                dataContext.order.Add(o);
                dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Podczas zapisywania zamówienia wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
                return;
            }

            //Load orders screen!
            _mainVM.LoadCustomAccountPage("orders");
        }
        private decimal ValueParser(string input)
        {
            string cut = input.Substring(input.IndexOf('(') + 1, input.LastIndexOf(')') - input.IndexOf('(') - 3);
            return decimal.Parse(cut, CultureInfo.InvariantCulture);
        }
        private decimal SumProducts()
        {
            decimal output = 0;

            foreach (var prod in Cart)
                output += prod.Price;

            return output;
        }
        private List<CartModel> GetCartList()
        {
            List<CartModel> output = new List<CartModel>();

            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                foreach (var key in _mainVM.Cart.Keys)
                {
                    CartModel prod = new CartModel();
                    prod.ProductId = key;
                    prod.ProductName = dataContext.product.FirstOrDefault(x => x.product_id == key).name;
                    prod.Price = dataContext.product.FirstOrDefault(x => x.product_id == key).price;
                    prod.Quantity = _mainVM.Cart[key];
                    output.Add(prod);
                }
            }
            catch (Exception e)
            {
                string mess = "Podczas uzyskiwania listy produktów z koszyka wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private List<string> GetCountries()
        {
            List<string> output = new List<string>();

            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                output = dataContext.country.OrderBy(x => x.country1).Select(x => x.country1).ToList();
            }
            catch (Exception e)
            {
                string mess = "Podczas uzyskiwania listy krajów wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void GetUserDefaultData()
        {
            address def = new address();
            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                def = dataContext.address.FirstOrDefault(x => x.address_id == dataContext.person.FirstOrDefault(
                    y => y.person_id == dataContext.account.FirstOrDefault(
                        z => z.account_id == AccountManager.LoggedId).person_id).address_id);
            }
            catch (Exception e)
            {
                string mess = "Podczas uzyskiwania domyślnych danych klienta wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            if (def == null) return;

            Road = def.road;
            City = def.city.city1;
            Country = def.city.country.country1;
            Zipcode = def.zip_code;
            House = def.house;
            Flat = def.flat;
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
