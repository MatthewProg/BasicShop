using BasicShop.Commands;
using BasicShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BasicShop.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private object _mainFrame;

        public object MainFrame
        {
            get { return _mainFrame; }
            set
            {
                if (value == _mainFrame) return;

                _mainFrame = value;
                OnPropertyChanged("MainFrame");
            }
        }

        public SimpleCommand CloseWindowCommand { get; set; }
        public SimpleCommand MinimalizeWindowCommand { get; set; }    
        public MainWindowViewModel(MainWindow main)
        {
            _mainWindow = main;

            CloseWindowCommand = new SimpleCommand(CloseWindow);
            MinimalizeWindowCommand = new SimpleCommand(MinimalizeWindow);

            var productList = new ProductList();
            (productList.DataContext as ProductListViewModel).CurrentSearch = null;
            (productList.DataContext as ProductListViewModel).CurrentCategoryId = 19;
            MainFrame = productList;

        }

        private void CloseWindow()
        {
            _mainWindow.Close();
        }

        private void MinimalizeWindow()
        {
            _mainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
