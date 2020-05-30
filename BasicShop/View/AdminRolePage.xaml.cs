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
    /// Interaction logic for AdminRolePage.xaml
    /// </summary>
    public partial class AdminRolePage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities();

        public AdminRolePage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("roleViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ctx.role.Load();
            viewSource.Source = ctx.role.Local;
            loading.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            role r = new role();
            ctx.role.Add(r);
            viewSource.View.MoveCurrentTo(r);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            role r = viewSource.View.CurrentItem as role;
            ctx.role.Remove(r);
            viewSource.View.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ctx.SaveChanges();
            viewSource.View.Refresh();
        }
    }
}
