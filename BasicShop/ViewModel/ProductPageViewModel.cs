using BasicShop.Commands;
using BasicShop.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicShop.ViewModel
{
    public class ProductPageViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;
        private string _name;
        private ObservableCollection<string> _images;
        private decimal _price;
        private string _description;
        private ObservableCollection<SpecifitationModel> _specification;
        private double _rating;
        private ObservableCollection<CommentModel> _comments;
        private string _selectedPhoto;
        private int _noOfAvaible;
        private ObservableCollection<Tuple<int, SimpleListModel>> _navList;
        private Visibility _loadingScreen;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public ObservableCollection<string> Images
        {
            get { return _images; }
            set
            {
                if (value == _images) return;

                _images = value;
                OnPropertyChanged("Images");
            }
        }
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value == _price) return;

                _price = value;
                OnPropertyChanged("Price");
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;

                _description = value;
                OnPropertyChanged("Description");
            }
        }
        public ObservableCollection<SpecifitationModel> Specification
        {
            get { return _specification; }
            set
            {
                if (value == _specification) return;

                _specification = value;
                OnPropertyChanged("Specification");
            }
        }
        public double Rating
        {
            get { return _rating; }
            set
            {
                if (value == _rating) return;

                _rating = value;
                OnPropertyChanged("Rating");
            }
        }
        public ObservableCollection<CommentModel> Comments
        {
            get { return _comments; }
            set
            {
                if (value == _comments) return;

                _comments = value;
                OnPropertyChanged("Comments");
            }
        }
        public string SelectedPhoto
        {
            get { return _selectedPhoto; }
            set
            {
                if (value == _selectedPhoto) return;

                _selectedPhoto = value;
                OnPropertyChanged("SelectedPhoto");
            }
        }
        public int NoOfAvaible
        {
            get { return _noOfAvaible; }
            set
            {
                if (value == _noOfAvaible) return;

                _noOfAvaible = value;
                OnPropertyChanged("NoOfAvaible");
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
        public int? CurrentProductId { get; set; }
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

        public SimpleCommand GoBackCommand { get; set; }
        public SimpleCommand GoHomeCommand { get; set; }
        public ParameterCommand ChangeImageCommand { get; set; }
        public ParameterCommand ChangeCategoryCommand { get; set; }

        public ProductPageViewModel()
        {
            GoBackCommand = new SimpleCommand(GoBack);
            GoHomeCommand = new SimpleCommand(GoHome);
            ChangeCategoryCommand = new ParameterCommand(ChangeCategory);
            ChangeImageCommand = new ParameterCommand(ChangeImage);

            Name = "Name";
            Images = new ObservableCollection<string>();
            Price = 10.0M;
            Description = "Description";
            Specification = new ObservableCollection<SpecifitationModel>();
            Rating = 5.0D;
            Comments = new ObservableCollection<CommentModel>();
            SelectedPhoto = string.Empty;
            NavList = new ObservableCollection<Tuple<int, SimpleListModel>>();
        }
        public ProductPageViewModel(MainWindowViewModel mvm, int? productId) : this()
        {
            if(productId == null)
            {
                StandardMessages.Error("Błędny numer produktu!");
                return;
            }
            _mainVM = mvm;
            LoadingScreenProcess(() =>
            {
                GetInfo(productId);
            });
        }


        private void ChangeImage(object param)
        {
            SelectedPhoto = param as string;
        }
        private void ChangeCategory(object param)
        {
            var obj = param as SimpleListModel;
            _mainVM.LoadProductList(obj.Id, null);
        }
        private void GoHome()
        {
            _mainVM.LoadPage("home");
        }
        private void GoBack()
        {
            _mainVM.NavigationGoBack();
        }
        private void GetInfo(int? prodId)
        {
            product obj = new product();

            try
            {
                var dataContext = new shopEntities();
                obj = dataContext.product.Where(x => x.product_id == prodId).ToList()[0];

            }
            catch(Exception e)
            {
                StandardMessages.Error(e.Message);
            }

            Name = obj.name;
            Images = new ObservableCollection<string>(GetProductImages(prodId));
            Price = obj.price;
            Description = obj.description;
            Specification = new ObservableCollection<SpecifitationModel>(GetProductSpecification(prodId));
            Rating = GetProductRating(prodId);
            Comments = new ObservableCollection<CommentModel>(GetProductComments(prodId));
            SelectedPhoto = Images[0];
            NoOfAvaible = GetNoOfAvaibleProducts(prodId);
            UpdateNavList(prodId);
        }
        private List<string> GetProductImages(int? prodId)
        {
            List<string> output = new List<string>();

            try
            {
                var dataContext = new shopEntities();
                output.Add(dataContext.product.Where(x => x.product_id == prodId).Select(x => x.thumbnail).ToList()[0]);
                output.AddRange(dataContext.product_image.Where(x => x.product_id == prodId).Select(x => x.image_src).ToList());
            }
            catch(Exception e)
            {
                StandardMessages.Error(e.Message);
            }

            return output;
        }
        private List<SpecifitationModel> GetProductSpecification(int? prodId)
        {
            List<SpecifitationModel> output = new List<SpecifitationModel>();

            try
            {
                var dataContext = new shopEntities();
                var obj = dataContext.productSpecification(prodId).ToList();
                foreach(var spec in obj)
                    output.Add(new SpecifitationModel() { Element = spec.SpecKey, Value = spec.SpecValue });
            }
            catch(Exception e)
            {
                StandardMessages.Error(e.Message);
            }

            return output;
        }
        private int GetNoOfAvaibleProducts(int? prodId)
        {
            int output = 0;
            try
            {
                var dataContext = new shopEntities();
                output = dataContext.warehouse.Where(x => x.product_id == prodId).Select(x => x.quantity).Sum();
            }
            catch(Exception e)
            {
                StandardMessages.Error(e.Message);
            }

            return output;
        }
        private double GetProductRating(int? prodId)
        {
            double output = 0;

            try
            {
                var dataContext = new shopEntities();
                if (dataContext.feedback.Where(x => x.product_id == prodId).Select(x => x.feedback_id).ToArray().Length > 0)
                    output = dataContext.feedback.Where(x => x.product_id == prodId).Average(x => x.rating);
                else
                    output = 0;
            }
            catch (InvalidOperationException)
                { StandardMessages.Error("Błędny numer produktu!"); }
            catch (Exception e)
                { StandardMessages.Error(e.Message); }

            return output;
            }
        private List<CommentModel> GetProductComments(int? prodId)
        {
            List<CommentModel> output = new List<CommentModel>();

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
        private void UpdateNavList(int? prodId)
        {
            var dataContext = new shopEntities();
            int? cat = null;
            try
            {
                cat = dataContext.product.Where(x => x.product_id == prodId).Select(x => x.category_id).ToList()[0];
                if (cat == null) throw new Exception();
            }
            catch(Exception e)
            {
                StandardMessages.Error("Błędny numer produktu!");
                NavList = new ObservableCollection<Tuple<int, SimpleListModel>>();
                return;
            }

            int counter = -1;
            var list = new ObservableCollection<Tuple<int, SimpleListModel>>();
            var catId = cat;
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
                    catId = parentId.ToArray()[0];
                } while (catId != null);
                NavList = new ObservableCollection<Tuple<int, SimpleListModel>>(list.OrderByDescending(p => p.Item1));
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
