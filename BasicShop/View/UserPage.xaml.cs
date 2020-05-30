using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            this.DataContext = new UserViewModel();
        }
    }
}
