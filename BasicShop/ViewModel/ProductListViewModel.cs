using BasicShop.Commands;
using BasicShop.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

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
        private string _currentSearch;
        private Visibility _searchVisibility;
        private ObservableCollection<Tuple<int, SimpleListModel>> _navList;
        private uint? _currentCategoryId;

        public ObservableCollection<Tuple<int, SimpleListModel>> NavList
        {
            get { return _navList; }
            set
            {
                if (value == _navList) return;

                _navList = value;
                OnPropertyChanged("NavList");
            }
        }
        public string AdditionalInfoText { get; private set; }
        public Visibility AdditionalInfoVisibility { get; private set; }
        public uint? CurrentCategoryId
        {
            get { return _currentCategoryId; }
            set
            {
                if (value == _currentCategoryId) return;

                _currentCategoryId = value;
                OnPropertyChanged("CurrentCategoryId");
            }
        }
        public Visibility SearchVisibility
        {
            get { return _searchVisibility; }
            set
            {
                if (value == _searchVisibility) return;

                _searchVisibility = value;
                OnPropertyChanged("SearchVisibility");
            }
        }
        public string CurrentSearch
        {
            get { return _currentSearch; }
            set
            {
                if (value == _currentSearch) return;

                _currentSearch = value;
                OnPropertyChanged("CurrentSearch");

                if (_currentSearch == null) SearchVisibility = Visibility.Collapsed;
                else SearchVisibility = Visibility.Visible;
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
                foreach (var e in _filters)
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
            Action mainLoad = () =>
            {
                SearchVisibility = Visibility.Collapsed;
                SetAdditionalInfo(null);
                UpdateNavList();
                SelectedShowingNumber = "15";
                LoadProductsFromDB();
                CreateSortingList();
                CreateShowingItemsComboBox();
            };
            LoadingScreenProcess(mainLoad);
            FilterCommand = new SimpleCommand(FilterList);
            SelectedItemChangedCommand = new SimpleCommand(ProcessSortingChange);

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
            
            /* - - - - - - - - - - - - - - - - - - - - - -*/

        }


        public void FilterList()
        {
            //DialogHost.CloseDialogCommand.Execute(new object(), null);
            //MessageBox.Show("ok");
        }

        private void SetAdditionalInfo(string text)
        {
            if (text == null)
            {
                AdditionalInfoVisibility = Visibility.Collapsed;
                AdditionalInfoText = string.Empty;
            }
            else
            {
                AdditionalInfoVisibility = Visibility.Visible;
                AdditionalInfoText = text;
            }

            OnPropertyChanged("AdditionalInfoVisibility");
            OnPropertyChanged("AdditionalInfoText");
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

        private void UpdateNavList()
        {
            if (CurrentCategoryId == null)
            {
                NavList = new ObservableCollection<Tuple<int, SimpleListModel>>();
                return;
            }

            int counter = -1;
            var list = new ObservableCollection<Tuple<int, SimpleListModel>>();
            var dataContext = new shopEntities();
            var catId = CurrentCategoryId;
            do
            {
                counter++;
                var categoryName = from l in dataContext.category
                               where l.category_id == catId
                               select l.name;
                list.Add(new Tuple<int, SimpleListModel>(counter, new SimpleListModel() { Id = (uint)catId, Name = categoryName.First() }));
                var parentId = from l in dataContext.category
                           where l.category_id == catId
                           select l.parent_category;
                catId = (uint?)parentId.FirstOrDefault();
            } while (catId != null);
            NavList = new ObservableCollection<Tuple<int, SimpleListModel>>(list.OrderByDescending(p => p.Item1));
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
            LoadingScreenProcess(LoadProductsFromDB);
        }

        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }

        private void LoadProductsFromDB()
        {

            var dataContext = new shopEntities();
            IQueryable<product> query = null;
            if(CurrentCategoryId != null)
                query = from l in dataContext.product
                        where l.category_id == CurrentCategoryId
                        select l;
            if (CurrentSearch != null)
            {
                if (query != null)
                    query = from l in query
                            where l.name.ToLower().Contains(CurrentSearch.ToLower())
                            select l;
                else
                    query = from l in dataContext.product
                            where l.name.ToLower().Contains(CurrentSearch.ToLower())
                            select l;
            }
            if(query == null)
                query = from l in dataContext.product
                        select l;

            SortingCategoryModel selected = new SortingCategoryModel();
            if (SelectedSortingName != null)
                selected = SortingList.FirstOrDefault(s => s.Name == SelectedSortingName);
            switch (selected.Sort.PropertyName)
            {
                case "name":
                    if (selected.Sort.Direction == ListSortDirection.Ascending)
                        query = from q in query
                                orderby q.name ascending
                                select q;
                    else
                        query = from q in query
                                orderby q.name descending
                                select q;
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
            //Products = new ObservableCollection<ProductListModel>();
            var tmp = new ObservableCollection<ProductListModel>();
            foreach (var e in list)
            {
                var p = new ProductListModel()
                {
                    Name = e.name,
                    Price = e.price,
                    ImageUrl = e.thumbnail
                };
                p.SetSpecification(e.specification);
                tmp.Add(p);
            }

            Products = tmp;
            if (tmp.Count == 0) SetAdditionalInfo("Nie znaleziono produktów pasujących do kryteriów wyszukiwania!");
            else SetAdditionalInfo(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
