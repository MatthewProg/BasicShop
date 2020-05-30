using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BasicShop.View
{
    /// <summary>
    /// Interaction logic for AdminFeedbackPage.xaml
    /// </summary>
    public partial class AdminFeedbackPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities(DatabaseHelper.GetConnectionString());

        public AdminFeedbackPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("feedbackViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() =>
            {
                ctx.feedback.Load();
            }, () =>
            {
                viewSource.Source = ctx.feedback.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            feedback f = new feedback();
            ctx.feedback.Add(f);
            viewSource.View.MoveCurrentTo(f);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            feedback f = viewSource.View.CurrentItem as feedback;
            ctx.feedback.Remove(f);
            viewSource.View.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ctx.SaveChanges();
            viewSource.View.Refresh();
        }

        private async void LoadScreenProcess(Action action, Action cont = null)
        {
            loading.Visibility = Visibility.Visible;

            await Task.Factory.StartNew(action);

            if (cont != null)
                cont();

            loading.Visibility = Visibility.Collapsed;
        }
    }
}
