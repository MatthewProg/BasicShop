using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminAddressPage.xaml
    /// </summary>
    public partial class AdminAddressPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities(DatabaseHelper.GetConnectionString());

        public AdminAddressPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("addressViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() =>
            {
                ctx.address.Load();
            }, () =>
            {
                viewSource.Source = ctx.address.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            address a = new address();
            ctx.address.Add(a);
            viewSource.View.MoveCurrentTo(a);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            address a = viewSource.View.CurrentItem as address;
            ctx.address.Remove(a);
            viewSource.View.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ctx.SaveChanges();
            viewSource.View.Refresh();
        }

        private async void LoadScreenProcess(Action action, Action cont = null)
        {
            loading.Visibility = Visibility.Visible;

            await Task.Factory.StartNew(action);

            if (cont != null)
                cont();

            loading.Visibility = Visibility.Collapsed;
        }
    }
}
