using BasicShop.Commands;
using BasicShop.Model;
using BasicShop.View;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop.ViewModel
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        private Visibility _loadingScreen;
        private MainWindowViewModel _host;
       
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
        public TreeViewCategoryModel SelectedCategory { get; set; }
        public ObservableCollection<TreeViewCategoryModel> CategoriesHierarchy { get; set; }

        public SimpleCommand TreeViewSelectionCommand { get; set; }
        public CategoriesViewModel(MainWindowViewModel host)
        {
            LoadingScreen = Visibility.Collapsed;
            _host = host;

            CategoriesHierarchy = new ObservableCollection<TreeViewCategoryModel>();

            TreeViewSelectionCommand = new SimpleCommand(GoToCategory);

            LoadingScreenProcess(GetCategoriesTree);

            // - - - - - - TESTS ONLY - - - - - -
            
            // - - - - - - - - - - - - - - - - - -
        }

        private void GetCategoriesTree()
        {
            try
            {
                var dataContext = new shopEntities();

                var tmp = new List<TreeViewCategoryModel>();

                foreach (var parentCategory in dataContext.category.Where(x => x.parent_category == null).ToList())
                {
                    var cat = new TreeViewCategoryModel();
                    cat.Name = parentCategory.name;
                    cat.CategoryId = parentCategory.category_id;
                    cat.Subcategories = GetChildCategory(parentCategory.category_id, dataContext.category.ToList());
                    tmp.Add(cat);
                }

                CategoriesHierarchy = new ObservableCollection<TreeViewCategoryModel>(tmp);
                OnPropertyChanged("CategoriesHierarchy");
            }
            catch(Exception e)
            {
                string mess = "Podczas ładowania listy kategorii wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }
        private List<TreeViewCategoryModel> GetChildCategory(int parentId, List<category> categoriesList)
        {
            var output = new List<TreeViewCategoryModel>();
            var tmp = categoriesList.Where(x => x.parent_category == parentId).ToList();

            if (tmp.Count == 0) return new List<TreeViewCategoryModel>();

            foreach (var el in tmp)
            {
                var cat = new TreeViewCategoryModel();
                cat.Name = el.name;
                cat.CategoryId = el.category_id;
                cat.Subcategories = GetChildCategory(el.category_id, categoriesList);
                output.Add(cat);
            }

            return output;
        }
        private void GoToCategory()
        {
            if (SelectedCategory == null) return;
            _host.LoadProductList((uint)SelectedCategory.CategoryId, null);
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
