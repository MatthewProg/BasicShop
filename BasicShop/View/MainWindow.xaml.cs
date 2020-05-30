using BasicShop.ViewModel;
using System.Windows;
using System.Windows.Input;

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
