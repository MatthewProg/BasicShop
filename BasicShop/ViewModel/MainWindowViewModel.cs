using BasicShop.Commands;
using BasicShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
        public ParameterCommand LoadScreenCommand { get; set; }
        public ParameterCommand SearchCommand { get; set; }
        public MainWindowViewModel(MainWindow main)
        {
            _mainWindow = main;

            CloseWindowCommand = new SimpleCommand(CloseWindow);
            MinimalizeWindowCommand = new SimpleCommand(MinimalizeWindow);
            LoadScreenCommand = new ParameterCommand(LoadScreen);
            SearchCommand = new ParameterCommand(Search);

            MainFrame = new InformationPage();
        }

        public void LoadProductList(uint? categoryID, string search)
        {
            MainFrame = new ProductList(categoryID, search);
        }
        private void LoadScreen(object param)
        {
            var txt = param as string;
            switch (txt)
            {
                case "home":
                    MainFrame = new InformationPage();
                    break;
                case "categories":
                    MainFrame = new CategoriesPage(this);
                    break;
                default:
                    break;
            }
        }
        private void Search(object param)
        {
            var search = param as string;
            if (search == null || search == string.Empty) return;

            LoadProductList(null, search);
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
