using BasicShop.Commands;
using BasicShop.Managers;
using BasicShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicShop.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel _mainVM;
        private object _frameView;
        private Visibility _adminSectionVisibility;
        private Visibility _loadingScreen;

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
        public Visibility AdminSectionVisibility
        {
            get { return _adminSectionVisibility; }
            set
            {
                if (value == _adminSectionVisibility) return;

                _adminSectionVisibility = value;
                OnPropertyChanged("AdminSectionVisibility");
            }
        }
        public Visibility LoadingScreen
        {
            get { return _loadingScreen; }
            set
            {
                if (value == _loadingScreen) return;

                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
        }

        public ParameterCommand ChangeViewCommand { get; set; }
        public SimpleCommand LogoutCommand { get; set; }

        public AccountViewModel(MainWindowViewModel mvm, string startingPage = "account")
        {
            _mainVM = mvm;

            AdminSectionVisibility = Visibility.Collapsed;
            LogoutCommand = new SimpleCommand(Logout);
            ChangeViewCommand = new ParameterCommand(ChangeView);

            FrameView = new UserPage();

            LoadingScreenProcess(() =>
            {
                CheckUserRole();
            }, () => 
            { 
                ChangeView(startingPage);
            });
        }

        private void CheckUserRole()
        {
            try
            {
                var dataContext = new shopEntities();
                var a = dataContext.account.FirstOrDefault(x => x.account_id == AccountManager.LoggedId);
                if (a.role_id == 2)
                    AdminSectionVisibility = Visibility.Visible;
                else
                    AdminSectionVisibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                string mess = "Podczas uzyskiwania uprawnień wystąpił błąd!\n";
                StandardMessages.Error(mess + e.Message);
            }
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
                case "adminRole":
                    FrameView = new AdminRolePage();
                    break;
                case "adminPosition":
                    FrameView = new AdminPositionPage();
                    break;
                case "adminAccount":
                    FrameView = new AdminAccountPage();
                    break;
                case "adminAddress":
                    FrameView = new AdminAddressPage();
                    break;
                case "adminStaff":
                    FrameView = new AdminStaffPage();
                    break;
                case "adminOrders":
                    FrameView = new AdminOrdersPage();
                    break;
                case "adminFeedback":
                    FrameView = new AdminFeedbackPage();
                    break;
                case "adminProduct":
                    FrameView = new AdminProductPage();
                    break;
                case "adminShop":
                    FrameView = new AdminShopPage();
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
        private async void LoadingScreenProcess(Action action, Action onMain = null)
        {
            LoadingScreen = Visibility.Visible;

            await Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });

            if (onMain != null)
                onMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
