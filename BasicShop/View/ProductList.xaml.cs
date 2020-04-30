using BasicShop.Model;
using BasicShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        public ProductList()
        {
            InitializeComponent();

            var ok = new SampleViewModel()
            {
                Code = "Code",
                Description = "Desc",
                Name = "Name"
            };
            testOnly.ItemsSource = new ObservableCollection<SampleViewModel>() {
                new SampleViewModel()
                {
                    Code = "Code",
                    Description = "Desc",
                    Name = "Name"
                },
                new SampleViewModel()
                {
                    Code="Secon",
                    Description="Desc2",
                    Name="Test"
                },ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok,ok
            };

            this.DataContext = new CheckListViewModel()
            {
                Checks = new ObservableCollection<CheckListModel>()
                {
                    new CheckListModel() { Name = "Test1" },
                    new CheckListModel() { Name = "Test2" },
                    new CheckListModel() { Name = "Test3" }
                }
            };
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
