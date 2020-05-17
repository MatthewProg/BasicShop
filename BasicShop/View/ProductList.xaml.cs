using BasicShop.Model;
using BasicShop.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        public ProductList()
        {
            InitializeComponent();
            this.DataContext = new ProductListViewModel();
            /*var ok = new SampleViewModel()
            {
                Code = "Code",
                Description = "Desc",
                Name = "Name"
            };
            testOnly.ItemsSource = new ObservableCollection<SampleViewModel>() {
                new SampleViewModel()
                {
                    Code = "Code",
                    Description = "Desc",
                    Name = "Name"
                },
                new SampleViewModel()
                {
                    Code="Secon",
                    Description="Desc2",
                    Name="Test"
                },ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok
            };

            var lis = new List<CheckListModel>()
            {
                new CheckListModel() { Name = "Test1" },
                new CheckListModel() { Name = "Test2" },
                new CheckListModel() { Name = "Test3" }
            };

            BasicShop.Model.ProductListModel model = new Model.ProductListModel();
            string testString = "Power:123;Height [mm]:9;";
            model.SetSpecification(testString);*/

            /*filtersPanel.DataContext = new ProductListViewModel()
            {
                SimpleFilters = new ObservableCollection<CheckListViewModel>()
                {
                    new CheckListViewModel(lis, "Test"),
                    new CheckListViewModel(lis, "Test2"),
                    new CheckListViewModel(lis, "Ojk"),
                    new CheckListViewModel(lis, "Dziala")
                }
            };*/
            //filtersPanel.DataContext = new CheckListViewModel(lis, "Test");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DrawerHost.CloseDrawerCommand.Execute(Dock.Left, leftDrawer);
        }
    }
}
