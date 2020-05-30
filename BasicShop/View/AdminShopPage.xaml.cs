using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminShopPage.xaml
    /// </summary>
    public partial class AdminShopPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities(DatabaseHelper.GetConnectionString());

        public AdminShopPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("storeViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() =>
            {
                ctx.store.Load();
            }, () =>
            {
                viewSource.Source = ctx.store.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            store s = new store();
            ctx.store.Add(s);
            viewSource.View.MoveCurrentTo(s);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            store s = viewSource.View.CurrentItem as store;
            ctx.store.Remove(s);
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
