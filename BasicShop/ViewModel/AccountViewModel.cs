using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.View;
using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            ChangeView(startingPage);
        }

        private void ChangeView(object param)
        {
            string change = param as string;

            switch (change)
            {
                case "account":
                    FrameView = new object();
                    break;
                case "orders":
                    FrameView = new OrdersPage();
                    break;
                case "delivery":
                    FrameView = new object();
                    break;
                case "whishlist":
                    FrameView = new object();
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
