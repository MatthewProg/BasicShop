using BasicShop.Commands;
using BasicShop.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicShop.ViewModel
{
    public class ProductListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CheckListViewModel> _filters;
        private ObservableCollection<ProductListModel> _products;
        private ObservableCollection<SortingCategoryModel> _sortingList;
        private ObservableCollection<Model.ComboBoxItemModel> _showingItems;
        private string _selectedSortingName;
        private string _selectedShowingNumber;
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

        public string SelectedShowingNumber
        {
            get { return _selectedShowingNumber; }
            set
            {
                if (value == _selectedShowingNumber) return;

                _selectedShowingNumber = value;
                OnPropertyChanged("SelectedShowingNumber");
            }
        }

        public ObservableCollection<Model.ComboBoxItemModel> ShowingItems
        {
            get { return _showingItems; }
            set
            {
                if (value == _showingItems) return;

                _showingItems = value;
                OnPropertyChanged("ShowingItems");
            }
        }
        public ObservableCollection<SortingCategoryModel> SortingList
        {
            get { return _sortingList; }
            set
            {
                if (value == _sortingList) return;

                _sortingList = value;
                OnPropertyChanged("SortingList");
            }
        }

        public string SelectedSortingName
        {
            get { return _selectedSortingName; }
            set
            {
                if (value == _selectedSortingName) return;

                _selectedSortingName = value;
                OnPropertyChanged("SelectedSortingName");
            }
        }

        public ObservableCollection<CheckListViewModel> Filters
        {
            get { return _filters; }
            set
            {
                if (value == _filters) return;

                _filters = value;
                OnPropertyChanged("Filters");
                OnPropertyChanged("ActiveFilters");
            }
        }

        public ObservableCollection<ProductListModel> Products
        {
            get { return _products; }
            set
            {
                if (value == _products) return;

                _products = value;
                OnPropertyChanged("Products");
            }
        }

        public ObservableCollection<SpecifitationModel> ActiveFilters
        {
            get
            {
                var tmp = new ObservableCollection<SpecifitationModel>();
                foreach(var e in _filters)
                {
                    foreach (var f in e.GetActiveFilters())
                        tmp.Add(f);
                }
                return tmp;
            }
        }

        public SimpleCommand FilterCommand { get; set; }
        public SimpleCommand SelectedItemChangedCommand { get; set; }


        public ProductListViewModel()
        {
            LoadingScreen = Visibility.Visible;

            SelectedShowingNumber = "15";
            LoadProductsFromDB();
            CreateSortingList();
            CreateShowingItemsComboBox();
            FilterCommand = new SimpleCommand(FilterList);
            SelectedItemChangedCommand = new SimpleCommand(ProcessSortingChange);

            LoadingScreen = Visibility.Collapsed;

            /* - - - - - - TESTS ONLY! - - - - - - -*/
            var li1 = new List<CheckListModel>()
            {
                new CheckListModel() { Name = "ok"},
                new CheckListModel() { Name = "dwa"},
                new CheckListModel() { Name = "three"},
                new CheckListModel() { Name = "4"}
            };
            var li2 = new List<CheckListModel>()
            {
                new CheckListModel() { Name = "ok"},
                new CheckListModel() { Name = "dwa"},
                new CheckListModel() { Name = "three"},
                new CheckListModel() { Name = "4"}
            };

            Filters = new ObservableCollection<CheckListViewModel>()
            {
                new CheckListViewModel(li1, "test1"),
                new CheckListViewModel(li2, "test2")
            };


            /*var p1 = new ProductListModel()
            {
                Name = "Drukarka",
                ImageUrl = @"./Category/11/1.jpg",
                Price = System.Data.SqlTypes.SqlMoney.Parse("100.00")
            };
            p1.SetSpecification("Wysokość [m]:10;Moc [W]:1200;");

            var p2 = new ProductListModel()
            {
                Name = "Drukarka",
                ImageUrl = @"./Category/19/2.jpg",
                Price = System.Data.SqlTypes.SqlMoney.Parse("99.99")
            };
            p2.SetSpecification("Kolor:zielony;Masa [kg]:10;");

            var p3 = new ProductListModel()
            {
                Name = "Drukarka",
                ImageUrl = @"./Category/8/1.jpg",
                Price = System.Data.SqlTypes.SqlMoney.Parse("9100.00")
            };
            p3.SetSpecification("Interfejs:SATA;Przekątna [cal]:12;");

            Products = new ObservableCollection<ProductListModel>()
            {
               p1,p2,p3
            };*/
            /* - - - - - - - - - - - - - - - - - - - - - -*/

        }


        public void FilterList()
        {
            //DialogHost.CloseDialogCommand.Execute(new object(), null);
            MessageBox.Show("ok");
        }

        private void CreateSortingList()
        {
            SortingList = new ObservableCollection<SortingCategoryModel>()
            {
                new SortingCategoryModel()
                {
                    Name = "Nazwa: A-Z",
                    IsSelected = true,
                    Sort = new SortDescription("name", ListSortDirection.Ascending)
                },
                new SortingCategoryModel()
                {
                    Name = "Nazwa: Z-A",
                    IsSelected = false,
                    Sort = new SortDescription("name", ListSortDirection.Descending)
                },
                new SortingCategoryModel()
                {
                    Name = "Cena: od najwyższej",
                    IsSelected = false,
                    Sort = new SortDescription("price", ListSortDirection.Descending)
                },
                new SortingCategoryModel()
                {
                    Name = "Cena: od najniższej",
                    IsSelected = false,
                    Sort = new SortDescription("price", ListSortDirection.Ascending)
                }
            };
        }

        private void CreateShowingItemsComboBox()
        {
            ShowingItems = new ObservableCollection<Model.ComboBoxItemModel>()
            {
                new Model.ComboBoxItemModel()
                {
                    Name = "15",
                    IsSelected = true,
                    Value = 15
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "30",
                    IsSelected = true,
                    Value = 30
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "45",
                    IsSelected = true,
                    Value = 45
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "90",
                    IsSelected = true,
                    Value = 90
                }
            };
        }

        private void ProcessSortingChange()
        {
            LoadingScreen = Visibility.Visible;

            LoadProductsFromDB();

            LoadingScreen = Visibility.Collapsed;
        }

        private void LoadProductsFromDB()
        {
            var dataContext = new shopEntities();
            var query = from l in dataContext.product
                        where l.category_id == 9
                        select l;

            SortingCategoryModel selected = new SortingCategoryModel();
            if(SelectedSortingName != null)
                selected = SortingList.FirstOrDefault(s => s.Name == SelectedSortingName);
            switch(selected.Sort.PropertyName)
            {
                case "name":
                    if (selected.Sort.Direction == ListSortDirection.Ascending)
                        query = from q in query
                                orderby q.name ascending select q;
                    else
                        query = from q in query
                                orderby q.name descending select q;
                    break;
                case "price":
                    if (selected.Sort.Direction == ListSortDirection.Ascending)
                        query = from q in query
                                orderby q.price ascending
                                select q;
                    else
                        query = from q in query
                                orderby q.price descending
                                select q;
                    break;
                default:
                    query = from q in query
                        orderby q.product_id
                        select q;
                    break;
            }
            var list = query.Take(int.Parse(SelectedShowingNumber));
            Products = new ObservableCollection<ProductListModel>();
            foreach(var e in list)
            {
                var p = new ProductListModel()
                {
                    Name = e.name,
                    Price = e.price,
                    ImageUrl = e.thumbnail
                };
                p.SetSpecification(e.specification);
                Products.Add(p);
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
