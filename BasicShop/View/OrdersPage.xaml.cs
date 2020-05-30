using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            this.DataContext = new OrdersViewModel();
        }
    }
}
