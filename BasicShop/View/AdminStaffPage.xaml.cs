using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminStaffPage.xaml
    /// </summary>
    public partial class AdminStaffPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities();

        public AdminStaffPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("staffViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ctx.staff.Load();
            viewSource.Source = ctx.staff.Local;
            loading.Visibility = Visibility.Collapsed;
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
    }
}
