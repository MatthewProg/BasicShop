using BasicShop.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
            Keyboard.ClearFocus();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leftPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            iconSearch.Visibility = Visibility.Visible;
            searchBox.Visibility = Visibility.Collapsed;
            searchBox.Text = string.Empty;
            Keyboard.ClearFocus();
        }

        private void leftPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            iconSearch.Visibility = Visibility.Collapsed;
            searchBox.Visibility = Visibility.Visible;
            searchBox.Focus();
        }
    }
}
