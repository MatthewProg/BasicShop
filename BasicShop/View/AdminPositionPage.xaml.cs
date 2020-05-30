﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for AdminPositionPage.xaml
    /// </summary>
    public partial class AdminPositionPage : Page
    {
        CollectionViewSource viewSource;
        shopEntities ctx = new shopEntities();

        public AdminPositionPage()
        {
            InitializeComponent();
            viewSource = ((CollectionViewSource)(FindResource("positionViewSource")));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenProcess(() => 
            { 
                ctx.position.Load();
            }, ()=> 
            {
                viewSource.Source = ctx.position.Local;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            position p = new position();
            ctx.position.Add(p);
            viewSource.View.MoveCurrentTo(p);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            position p = viewSource.View.CurrentItem as position;
            ctx.position.Remove(p);
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
