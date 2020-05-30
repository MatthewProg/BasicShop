using BasicShop.ViewModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {

        public ProductList(MainWindowViewModel mvm, uint? category = null, string search = null)
        {
            InitializeComponent();
            this.DataContext = new ProductListViewModel(mvm, category, search);
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
