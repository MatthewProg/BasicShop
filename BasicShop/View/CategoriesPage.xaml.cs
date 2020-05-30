using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
        }

        public CategoriesPage(MainWindowViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = new CategoriesViewModel(mvm);
        }
    }
}
