using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http.Headers;
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
        private bool _inWishlist;
        private View.ProductPage _productPage;
        private ObservableCollection<WarehouseModel> _warehouses;

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
        public bool InWhishlist
        {
            get { return _inWishlist; }
            set
            {
                if (value == _inWishlist) return;

                _inWishlist = value;
                OnPropertyChanged("InWhishlist");
            }
        }
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }
        public ObservableCollection<WarehouseModel> Warehouses
        {
            get { return _warehouses; }
            set
            {
                if (value == _warehouses) return;

                _warehouses = value;
                OnPropertyChanged("Warehouses");
            }
        }

        public SimpleCommand GoBackCommand { get; set; }
        public SimpleCommand GoHomeCommand { get; set; }
        public SimpleCommand WhishlistCommand { get; set; }
        public ParameterCommand ChangeImageCommand { get; set; }
        public ParameterCommand ChangeCategoryCommand { get; set; }
        public ParameterCommand AddCommentCommand { get; set; }

        public ProductPageViewModel()
        {
            GoBackCommand = new SimpleCommand(GoBack);
            GoHomeCommand = new SimpleCommand(GoHome);
            WhishlistCommand = new SimpleCommand(ProcessWhishlistChange);
            ChangeCategoryCommand = new ParameterCommand(ChangeCategory);
            ChangeImageCommand = new ParameterCommand(ChangeImage);
            AddCommentCommand = new ParameterCommand(AddComment);

            Name = "Name";
            Images = new ObservableCollection<string>();
            Price = 0.0M;
            Description = "Description";
            Specification = new ObservableCollection<SpecifitationModel>();
            Rating = 0.0D;
            Comments = new ObservableCollection<CommentModel>();
            SelectedPhoto = string.Empty;
            NavList = new ObservableCollection<Tuple<int, SimpleListModel>>();
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0,0,2));
        }
        public ProductPageViewModel(View.ProductPage page, MainWindowViewModel mvm, int? productId) : this()
        {
            if(productId == null)
            {
                string mess = "Podczas ładowania produktu wystąpił błąd!\n";
                StandardMessages.Error(mess + "Błędny numer produktu!");
                return;
            }
            _productPage = page;
            _mainVM = mvm;
            LoadingScreenProcess(() =>
            {
                GetInfo(productId);
            });
        }

        private void AddComment(object param)
        {
            object[] obj = new object[2];
            obj = (object[])param;

            int rating = (int)obj[0];
            string comment = (string)obj[1];

            if(rating == 0)
            {
                MessageQueue.Enqueue("Należy podać wartość oceny");
                return;
            }
            if (AccountManager.LoggedId == null)
            {
                MessageQueue.Enqueue("Musisz być zalogowany, aby dodawać opinie!");
                return;
            }

            feedback comm = new feedback();
            comm.account_id = (int)AccountManager.LoggedId;
            comm.comment = (comment == null || comment == string.Empty) ? null : comment;
            comm.product_id = (int)CurrentProductId;
            comm.rating = rating;

            try
            {
                var dataContext = new shopEntities();
                dataContext.feedback.Add(comm);
                dataContext.SaveChanges();
            }
            catch(Exception e)
            {
                string mess = "Podczas dodawania opinii wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            Rating = GetProductRating(CurrentProductId);
            Comments = new ObservableCollection<CommentModel>(GetProductComments(CurrentProductId));

            _productPage.newRating.Value = 0;
            _productPage.newComment.Text = string.Empty;
        }
        private void ProcessWhishlistChange()
        {
            if(AccountManager.LoggedId == null)
            {
                InWhishlist = false;
                MessageQueue.Enqueue("Musisz być zalogowany, aby dodać produkt do listy życzeń", null, null, null, true, false, new TimeSpan(0,0,4));
                return;
            }

            try
            {
                var dataContext = new shopEntities();
                if (IsInWhishlist(CurrentProductId))
                {
                    var p = dataContext.product.FirstOrDefault(x => x.product_id == CurrentProductId);
                    var a = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                    p.account.Remove(a);
                    dataContext.SaveChanges();
                    MessageQueue.Enqueue("Produkt został usunięty z listy życzeń");
                }
                else
                {
                    var p = dataContext.product.FirstOrDefault(x => x.product_id == CurrentProductId);
                    var a = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                    p.account.Add(a);
                    dataContext.SaveChanges();
                    MessageQueue.Enqueue("Produkt został dodany do listy życzeń");
                }       
            }
            catch(Exception e)
            {
                string mess = "Podczas edytowania listy życzeń wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }
        private bool IsInWhishlist(int? prodId)
        {
            bool output = false;

            if (AccountManager.LoggedId == null || prodId == null) return false;

            try
            {
                var dataContext = new shopEntities();
                var count = from a in dataContext.account
                            from p in a.product
                            where p.product_id == prodId && a.account_id == AccountManager.LoggedId
                           select new
                           {
                               account_id = a.account_id,
                               product_id = p.product_id
                           };
                if (count.Count() != 0) output = true;
            }
            catch(Exception e)
            {
                string mess = "Podczas edytowania listy życzeń wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
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
            _mainVM.LoadPage("categories");
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
                string mess = "Podczas uzyskiwania obiektu produktu wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            CurrentProductId = obj.product_id;
            Name = obj.name;
            Images = new ObservableCollection<string>(GetProductImages(prodId));
            Price = obj.price;
            Description = obj.description;
            Specification = new ObservableCollection<SpecifitationModel>(GetProductSpecification(prodId));
            Rating = GetProductRating(prodId);
            Comments = new ObservableCollection<CommentModel>(GetProductComments(prodId));
            SelectedPhoto = Images[0];
            NoOfAvaible = GetNoOfAvaibleProducts(prodId);
            InWhishlist = IsInWhishlist(prodId);
            Warehouses = new ObservableCollection<WarehouseModel>(GetWarehouses(prodId));
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
                string mess = "Podczas uzyskiwania zdjęć produktu wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
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
                string mess = "Podczas uzyskiwania specyfikacji produktu wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
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
                string mess = "Podczas uzyskiwania ilości dostępnych produktów wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
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
            {
                string mess = "Podczas uzyskiwania oceny produktu wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
            }
        private List<CommentModel> GetProductComments(int? prodId)
        {
            List<CommentModel> output = new List<CommentModel>();

            var obj = new List<feedback>();
            try
            {
                var dataContext = new shopEntities();
                obj = dataContext.feedback.Where(x => x.product_id == prodId).OrderByDescending(x=>x.feedback_id).ToList();

                foreach(var comment in obj)
                {
                    output.Add(new CommentModel()
                    {
                        CommentId = comment.feedback_id,
                        Comment = comment.comment,
                        Rating = comment.rating,
                        UserId = comment.account_id,
                        Username = dataContext.account.Where(x => x.account_id == comment.account_id).Select(x => x.username).ToArray()[0]
                    });
                }
            }
            catch(Exception e)
            {
                string mess = "Podczas uzystkiwania opinii wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
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
                string mess = "Podczas uzyskiwania numeru kategorii wystąpił błąd!\n";
                StandardMessages.Error(mess + "Błędny numer produktu!");
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
                string mess = "Podczas próby uzyskania wyższych kategorii wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }
        private List<WarehouseModel> GetWarehouses(int? prodId)
        {
            List<WarehouseModel> output = new List<WarehouseModel>();

            List<WarehouseAllInfoView> li = new List<WarehouseAllInfoView>();
            try
            {
                var dataContext = new shopEntities();
                li = dataContext.WarehouseAllInfoView.Where(x => x.product_id == prodId && x.quantity > 0).ToList();
            }
            catch (Exception e)
            {
                string mess = "Podczas uzystkiwania stanów magazynowych wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }

            foreach (var shop in li)
                output.Add(new WarehouseModel()
                {
                    Quantity = shop.quantity,
                    Email = shop.email,
                    Phone = shop.phone,
                    House = shop.house,
                    Flat = shop.flat,
                    Road = shop.road,
                    ZipCode = shop.zip_code.Substring(0,2) + "-" + shop.zip_code.Substring(2),
                    City = shop.city,
                    Country = shop.country
                });

            return output;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
