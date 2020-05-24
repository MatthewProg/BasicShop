using BasicShop.Commands;
using BasicShop.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Security.Authentication.ExtendedProtection.Configuration;
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
        private ObservableCollection<object> _filters;
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
        private Visibility _leftVisibility;
        private Visibility _rightVisibility;
        private int _currentPage;
        private int _countProducts;
        private ObservableCollection<SpecifitationModel> _activeFilters;

        public int CurrentPage
        {
            get { return _currentPage + 1; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
        public Visibility LeftVisibility
        {
            get { return _leftVisibility; }
            set
            {
                if (value == _leftVisibility) return;

                _leftVisibility = value;
                OnPropertyChanged("LeftVisibility");
            }
        }
        public Visibility RightVisibility
        {
            get { return _rightVisibility; }
            set
            {
                if (value == _rightVisibility) return;

                _rightVisibility = value;
                OnPropertyChanged("RightVisibility");
            }
        }
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
        public ObservableCollection<object> Filters
        {
            get { return _filters; }
            set
            {
                if (value == _filters) return;

                _filters = value;
                OnPropertyChanged("Filters");
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
            get{ return _activeFilters; }
            set
            {
                if (value == _activeFilters) return;

                _activeFilters = value;
                OnPropertyChanged("ActiveFilters");
            }
        }


        public SimpleCommand FilterCommand { get; set; }
        public SimpleCommand SelectedItemChangedCommand { get; set; }
        public ParameterCommand SwitchPageCommand { get; set; }
        public ParameterCommand DeleteFilterCommand { get; set; }
        public ParameterCommand ChangeCategoryCommand { get; set; }


        public ProductListViewModel(uint? category = null, string search = null)
        {
            ActiveFilters = new ObservableCollection<SpecifitationModel>();
            FilterCommand = new SimpleCommand(FilterList);
            SelectedItemChangedCommand = new SimpleCommand(ProcessSortingChange);
            SwitchPageCommand = new ParameterCommand(SetPage);
            DeleteFilterCommand = new ParameterCommand(DeleteFilter);
            ChangeCategoryCommand = new ParameterCommand(ChangeCategory);

            CurrentCategoryId = category;
            CurrentSearch = search;

            FullReload();
        }
       

        private void FullReload()
        {
            CurrentPage = 0;
            SearchVisibility = Visibility.Collapsed;
            CreateSortingList();
            CreateShowingItemsComboBox();
            SelectedShowingNumber = "15";

            Action mainLoad = () =>
            {
                SetAdditionalInfo(null);
                UpdateNavList();
                UpdateFilters();
                LoadProductsFromDB();
                SetPageNavVisibility();
            };
            LoadingScreenProcess(mainLoad);
        }
        private void ChangeCategory(object param)
        {
            var obj = param as SimpleListModel;
            CurrentCategoryId = obj.Id;
            FullReload();
        }
        private void FilterList()
        {
            if (_filters == null) return;
            var tmp = new ObservableCollection<SpecifitationModel>();
            foreach (var e in _filters)
            {
                if (e is CheckListViewModel)
                    foreach (var f in (e as CheckListViewModel).GetActiveFilters())
                        tmp.Add(f);
                else if (e is SliderListViewModel)
                {
                    var obj = e as SliderListViewModel;
                    if (obj.ValueMinimum != obj.Minimum) tmp.Add(new SpecifitationModel() { Element = obj.Header, Value = ">" + obj.ValueMinimum.ToString("F" + obj.Precision.ToString(), CultureInfo.InvariantCulture) });
                    if (obj.ValueMaximum != obj.Maximum) tmp.Add(new SpecifitationModel() { Element = obj.Header, Value = "<" + obj.ValueMaximum.ToString("F" + obj.Precision.ToString(), CultureInfo.InvariantCulture) });
                }
            }
            ActiveFilters = tmp;
            Action reload = () =>
            {
                LoadProductsFromDB();
                SetPageNavVisibility();
            };
            LoadingScreenProcess(reload);
        }
        private void DeleteFilter(object param)
        {
            var obj = param as SpecifitationModel;

            object filter = null;
            foreach(Interfaces.IFilter fil in _filters)
                if (fil.Header == obj.Element)
                {
                    filter = fil;
                    break;
                }

            if (obj.Value[0] == '>')
                (filter as SliderListViewModel).ValueMinimum = (filter as SliderListViewModel).Minimum;
            else if (obj.Value[0] == '<')
                (filter as SliderListViewModel).ValueMaximum = (filter as SliderListViewModel).Maximum;
            else
                foreach (var check in (filter as CheckListViewModel).Checks)
                    if (check.Name == obj.Value)
                    {
                        check.IsChecked = false;
                        break;
                    }
            ActiveFilters.Remove(obj);
            LoadingScreenProcess(LoadProductsFromDB);
        }
        private void SetPage(object param)
        {
            if (param == null) return;

            string direction = param.ToString();

            if(direction.ToLower() == "left")
            {
                if (_currentPage == 0) return;
                CurrentPage = _currentPage - 1;
            }
            else
            {
                if (_currentPage == Math.Ceiling(_countProducts / float.Parse(_selectedShowingNumber))) return;
                CurrentPage = _currentPage + 1;
            }

            SetPageNavVisibility();
            LoadingScreenProcess(LoadProductsFromDB);
        }
        private void SetPageNavVisibility()
        {
            LeftVisibility = Visibility.Visible;
            RightVisibility = Visibility.Visible;

            if (_currentPage == 0) LeftVisibility = Visibility.Hidden;
            if (CurrentPage == Math.Ceiling(_countProducts / float.Parse(_selectedShowingNumber)) || _countProducts <= float.Parse(_selectedShowingNumber)) RightVisibility = Visibility.Hidden;
        }
        private void UpdateFilters()
        {
            try
            {
                var working = new ObservableCollection<object>();
                var dataContext = new shopEntities();

                IQueryable<int?> li = null;
                IQueryable<product> query = null;
                if (CurrentCategoryId != null)
                {
                    li = dataContext.childCategories((int)CurrentCategoryId);
                    query = dataContext.product.Where(o => li.Contains((int)o.category_id));
                }
                if (CurrentSearch != null)
                {
                    IQueryable<int> sec = null;
                    if (li == null)
                        sec = dataContext.product.Where(x => x.name.Contains(CurrentSearch)).Select(x => x.product_id);
                    else
                        sec = dataContext.product.Where(x => x.name.Contains(CurrentSearch) && li.Contains(x.category_id)).Select(x => x.product_id);

                    query = dataContext.product.Where(o => sec.Contains((int)o.product_id));
                }

                try
                {
                    if (query.Select(x => x.product_id).First() == 0) return;
                }
                catch
                {
                    return;
                }

                var minPrice = query.Min(x => x.price);
                var maxPrice = query.Max(x => x.price);

                float st = 0.0F;
                if (maxPrice - minPrice <= 1) st = 0.01F;
                else if (maxPrice - minPrice <= 10) st = 0.1F;
                else if (maxPrice - minPrice <= 500) st = 1.0F;
                else if (maxPrice - minPrice <= 10000) st = 10.0F;
                else st = 20.0F;

                var priceSlider = new SliderListViewModel((float)minPrice, (float)maxPrice, st, "Cena");
                working.Add(priceSlider);
                //Sub categories filters also
                //var items = dataContext.categorySpecificationWithChildren((int)CurrentCategoryId);

                //Only this category filters
                if (CurrentCategoryId == null) { Filters = working; return; }
                var items = dataContext.categorySpecification((int)CurrentCategoryId);
                foreach (var specKey in items.Select(x => x.SpecKey).Distinct())
                {
                    //var section = new CheckListViewModel();
                    //section.Header = specKey;
                    List<float> variantsF = new List<float>();
                    List<string> variantsS = new List<string>();
                    bool onlyNumbers = true;
                    var fMin = float.MaxValue;
                    var fMax = float.MinValue;
                    foreach (var variant in items.Where(x => x.SpecKey == specKey).Select(x => x.SpecValue).Distinct())
                    {
                        float converted = 0.0F;
                        if (float.TryParse(variant, NumberStyles.Float, CultureInfo.InvariantCulture, out converted))
                        {
                            variantsF.Add(converted);
                            if (converted <= fMin) fMin = converted;
                            if (converted >= fMax) fMax = converted;
                        }
                        else
                            onlyNumbers = false;

                        variantsS.Add(variant);
                        //section.Checks.Add(new CheckListModel() { Name = variant });
                    }

                    if (!onlyNumbers)
                    {
                        var section = new CheckListViewModel();
                        section.Header = specKey;
                        foreach (var s in variantsS)
                            section.Checks.Add(new CheckListModel() { Name = s });
                        working.Add(section);
                    }
                    else
                    {
                        float step = 0.0F;
                        if (fMax - fMin <= 1) step = 0.01F;
                        else if (fMax - fMin <= 10) step = 0.1F;
                        else if (fMax - fMin <= 500) step = 1.0F;
                        else if (fMax - fMin <= 10000) step = 10.0F;
                        else step = 20.0F;
                        var section = new SliderListViewModel(fMin, fMax, step, specKey);
                        working.Add(section);
                    }
                }

                Filters = working;
            }
            catch(Exception e)
            {
                StandardMessages.Error(e.Message);
            }
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
            try
            {
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
                    catId = (uint?)parentId.ToArray()[0];
                } while (catId != null);
                NavList = new ObservableCollection<Tuple<int, SimpleListModel>>(list.OrderByDescending(p => p.Item1));
            }
            catch (Exception e)
            {
                StandardMessages.Error(e.Message);
            }
}
        /* Got replaced by SQL function
        BTW, it was garbage

        private List<int> GetChildCategoriesId(int parent)
        {
            var output = new List<int>();
            output.Add(parent);

            var dataContext = new shopEntities();
            int currentCount = 0;
            bool GetAndConcat()
            {
                currentCount = output.Count;
                List<int> small = new List<int>();
                foreach(var e in output)
                {
                    var query = from l in dataContext.category
                                where l.parent_category == e
                                select l.category_id;
                    small.AddRange(query.ToList());
                }
                output.AddRange(small);
                output = new List<int>(output.Distinct());
                return currentCount != output.Count;
            }

            while(GetAndConcat()) { ; }

            return output;
        }*/
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
                    IsSelected = false,
                    Value = 30
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "45",
                    IsSelected = false,
                    Value = 45
                },
                new Model.ComboBoxItemModel()
                {
                    Name = "90",
                    IsSelected = false,
                    Value = 90
                }
            };
        }
        private void ProcessSortingChange()
        {
            CurrentPage = 0;
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
            try
            {
                var dataContext = new shopEntities();
                IQueryable<product> query = null;

                //Get products from category
                if (CurrentCategoryId != null)
                {
                    var li = dataContext.childCategories((int)CurrentCategoryId);
                    query = dataContext.product.Where(o => li.Contains((int)o.category_id));
                }

                //Get products from name
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
                if (query == null)
                    query = from l in dataContext.product
                            select l;

                //Split active filters into category - values
                Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>();
                foreach (var filter in ActiveFilters)
                {
                    if (!filters.ContainsKey(filter.Element))
                        filters.Add(filter.Element, new List<string>());
                    filters[filter.Element].Add(filter.Value);
                }

                foreach (var key in filters.Keys)
                {
                    List<string> condition = new List<string>();
                    foreach (var val in filters[key])
                    {
                        if (val[0] != '>' && val[0] != '<')
                        {
                            condition.Add($"{key}:{val}");
                            query = query.Where(x => condition.Any(y => x.specification.Contains(y)));
                        }
                        else if (key == "Cena")
                        {
                            string min = null, max = null;
                            if (filters[key][0][0] == '>')
                            {
                                min = filters[key][0].Substring(1);
                                if (filters[key].Count > 1)
                                    max = filters[key][1].Substring(1);
                            }
                            else
                            {
                                max = filters[key][0].Substring(1);
                                if (filters[key].Count > 1)
                                    min = filters[key][1].Substring(1);
                            }
                            if (max != null)
                            {
                                decimal maxD = decimal.Parse(max);
                                query = query.Where(x => x.price <= maxD);
                            }
                            if (min != null)
                            {
                                decimal minD = decimal.Parse(min);
                                query = query.Where(x => x.price >= minD);
                            }
                        }
                        else
                        {
                            var products = query.Select(x => new { id = x.product_id, spec = x.specification });
                            var prodSpec = products.Where(x => x.spec.Contains(key)).ToList();
                            Dictionary<int, List<string>> specSplit = new Dictionary<int, List<string>>();
                            foreach (var element in prodSpec)
                            {
                                if (!specSplit.ContainsKey(element.id))
                                    specSplit.Add(element.id, new List<string>());
                                specSplit[element.id].AddRange(element.spec.Split(';'));
                            }
                            List<int> validateProductIds = new List<int>();
                            foreach (var k in specSplit.Keys)
                            {
                                var buff = specSplit[k].Where(x => x.Contains(key)).FirstOrDefault();
                                buff = buff.Split(':')[1];

                                if (val[0] == '<')
                                {
                                    if (decimal.Parse(buff, CultureInfo.InvariantCulture) <= decimal.Parse(val.Substring(1), CultureInfo.InvariantCulture))
                                        validateProductIds.Add(k);
                                }
                                else
                                {
                                    if (decimal.Parse(buff, CultureInfo.InvariantCulture) >= decimal.Parse(val.Substring(1), CultureInfo.InvariantCulture))
                                        validateProductIds.Add(k);
                                }
                            }

                            query = query.Where(x => validateProductIds.Any(y => x.product_id == y));
                        }
                    }
                }



                //Sort list
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

                //Page processing
                _countProducts = query.Count();
                var list = query.Skip(_currentPage * int.Parse(SelectedShowingNumber)).Take(int.Parse(SelectedShowingNumber));

                //Mapping into object
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

                //0 element list
                if (tmp.Count == 0) SetAdditionalInfo("Nie znaleziono produktów pasujących do kryteriów wyszukiwania!");
                else SetAdditionalInfo(null);
            }
            catch (Exception e)
            {
                StandardMessages.Error(e.Message);
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
