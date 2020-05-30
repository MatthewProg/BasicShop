using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage(MainWindowViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = new CartViewModel(mvm);
        }
    }
}
