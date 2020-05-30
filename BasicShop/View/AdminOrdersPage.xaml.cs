using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminOrdersPage.xaml
    /// </summary>
    public partial class AdminOrdersPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities(DatabaseHelper.GetConnectionString());

        public AdminOrdersPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("orderViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() =>
            {
                ctx.order.Load();
            }, () =>
            {
                viewSource.Source = ctx.order.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            order o = new order();
            ctx.order.Add(o);
            viewSource.View.MoveCurrentTo(o);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            order o = viewSource.View.CurrentItem as order;
            ctx.order.Remove(o);
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
