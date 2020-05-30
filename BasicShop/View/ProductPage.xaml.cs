using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Logika interakcji dla klasy PoductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
        }

        public ProductPage(MainWindowViewModel mvm, int? productId)
        {
            InitializeComponent();
            this.DataContext = new ProductPageViewModel(this, mvm, productId);
        }
    }
}
