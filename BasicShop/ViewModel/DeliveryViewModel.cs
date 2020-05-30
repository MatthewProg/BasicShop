using BasicShop.Commands;
using BasicShop.Managers;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class DeliveryViewModel : INotifyPropertyChanged
    {
        private Visibility _loadingScreen;
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
        public SnackbarMessageQueue MessageQueue { get; set; }

        public SimpleCommand SaveCommand { get; set; }

        public DeliveryViewModel()
        {
            MessageQueue = new SnackbarMessageQueue(new TimeSpan(0, 0, 2));
            SaveCommand = new SimpleCommand(Save);

            LoadingScreenProcess(() =>
            {
                Countries = new ObservableCollection<string>(GetCountries());
                LoadData();
            });
        }

        private void Save()
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

            MessageQueue.Enqueue("Zapisywanie..");
            RunInBackground(() =>
            {
                try
                {
                    var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());

                    var acc = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                    var query = dataContext.address.FirstOrDefault(x => x.road == Road && x.house == House && x.flat == Flat && x.zip_code == Zipcode && x.city.city1 == City);

                    if (query != null)
                        acc.person.address_id = query.address_id;
                    else
                    {
                        int cityId;
                        if (dataContext.city.Count(x => x.city1 == City) != 0)
                            cityId = dataContext.city.FirstOrDefault(x => x.city1 == City).city_id;
                        else
                        {
                            int countryId = dataContext.country.FirstOrDefault(x => x.country1 == Country).country_id;
                            dataContext.city.Add(new city() { city1 = City, country_id = countryId });
                            dataContext.SaveChanges();
                            cityId = dataContext.city.FirstOrDefault(x => x.city1 == City).city_id;
                        }

                        acc.person.address = new address();
                        acc.person.address.road = Road;
                        acc.person.address.city_id = cityId;
                        acc.person.address.flat = Flat;
                        acc.person.address.house = House;
                        acc.person.address.zip_code = Zipcode;
                    }
                    dataContext.account.AddOrUpdate(acc);
                    dataContext.SaveChanges();
                }
                catch (Exception e)
                {
                    string mess = "Podczas zapisywania zamówienia wystąpił błąd!\n";
                    StandardMessages.Error(mess + e.Message);
                    return;
                }
            }, () =>
            {
                MessageQueue.Enqueue("Zapisano!");
            });
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
        private void LoadData()
        {
            address a = new address();
            try
            {
                var dataContext = new shopEntities(DatabaseHelper.GetConnectionString());
                var acc = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                var p = acc.person;
                a = p.address;
            }
            catch (Exception e)
            {
                string mess = "Podczas ładowania danych wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
            if (a == null) return;
            Road = a.road;
            Zipcode = a.zip_code;
            House = a.house;
            Flat = a.flat;
            City = a.city.city1;
            Country = a.city.country.country1;
        }
        private async void LoadingScreenProcess(Action action, Action onMain = null)
        {
            LoadingScreen = Visibility.Visible;

            await Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });

            if (onMain != null)
                onMain();
        }
        private async void RunInBackground(Action action, Action onMain = null)
        {
            await Task.Factory.StartNew(action);
            if (onMain != null)
                onMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
