using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage(MainWindowViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(mvm);
        }
    }
}
