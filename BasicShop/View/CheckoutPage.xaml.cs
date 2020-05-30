using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        public CheckoutPage(MainWindowViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = new CheckoutViewModel(mvm);
        }
    }
}
