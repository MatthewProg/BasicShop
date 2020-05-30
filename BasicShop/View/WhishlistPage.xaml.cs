using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for WhishlistPage.xaml
    /// </summary>
    public partial class WhishlistPage : Page
    {
        public WhishlistPage(MainWindowViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = new WhishlistViewModel(mvm);
        }
    }
}
