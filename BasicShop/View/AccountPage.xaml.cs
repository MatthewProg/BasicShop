using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage(MainWindowViewModel mvm, string startingPage = "account")
        {
            InitializeComponent();
            this.DataContext = new AccountViewModel(mvm, startingPage);
        }
    }
}
