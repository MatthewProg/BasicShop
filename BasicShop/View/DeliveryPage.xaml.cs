using BasicShop.ViewModel;
using System.Windows.Controls;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for DeliveryPage.xaml
    /// </summary>
    public partial class DeliveryPage : Page
    {
        public DeliveryPage()
        {
            InitializeComponent();
            this.DataContext = new DeliveryViewModel();
        }
    }
}
