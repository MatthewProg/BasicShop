using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BasicShop.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;
        private object _frameView;

        public object FrameView
        {
            get { return _frameView; }
            set
            {
                if (value == _frameView) return;

                _frameView = value;
                OnPropertyChanged("FrameView");
            }
        }

        public ParameterCommand ChangeViewCommand { get; set; }
        public SimpleCommand LogoutCommand { get; set; }

        public AccountViewModel(MainWindowViewModel mvm, string startingPage = "account")
        {
            _mainVM = mvm;

            LogoutCommand = new SimpleCommand(Logout);
            ChangeViewCommand = new ParameterCommand(ChangeView);

            FrameView = new UserPage();

            ChangeView(startingPage);
        }

        private void ChangeView(object param)
        {
            string change = param as string;

            switch (change)
            {
                case "account":
                    FrameView = new UserPage();
                    break;
                case "orders":
                    FrameView = new OrdersPage();
                    break;
                case "delivery":
                    FrameView = new DeliveryPage();
                    break;
                case "whishlist":
                    FrameView = new WhishlistPage(_mainVM);
                    break;
                default:
                    break;
            }
        }
        private void Logout()
        {
            AccountManager.Logout();
            _mainVM.LoadPage("account");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
