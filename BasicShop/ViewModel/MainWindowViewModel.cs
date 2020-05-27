using BasicShop.Commands;
using BasicShop.Managers;
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

            LoadPage("home");
        }

        public void LoadPage(string name)
        {
            LoadScreen(name);
        }
        public void LoadProductList(uint? categoryID, string search)
        {
            MainFrame = new ProductList(this,categoryID, search);
        }
        public void LoadProduct(int? prodId)
        {
            MainFrame = new ProductPage(this, prodId);
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
                case "account":
                    if (AccountManager.LoggedId == null)
                        MainFrame = new LoginPage(this);
                    else
                        MainFrame = new AccountPage();
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
        public void NavigationGoBack()
        {
            if(_mainWindow.frameMain.CanGoBack)
                _mainWindow.frameMain.GoBack();
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
