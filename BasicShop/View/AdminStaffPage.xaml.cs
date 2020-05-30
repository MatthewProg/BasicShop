using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminStaffPage.xaml
    /// </summary>
    public partial class AdminStaffPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities(DatabaseHelper.GetConnectionString());

        public AdminStaffPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("staffViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() =>
            {
                ctx.staff.Load();
            }, () =>
            {
                viewSource.Source = ctx.staff.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            staff s = new staff();
            ctx.staff.Add(s);
            viewSource.View.MoveCurrentTo(s);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            staff s = viewSource.View.CurrentItem as staff;
            ctx.staff.Remove(s);
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
