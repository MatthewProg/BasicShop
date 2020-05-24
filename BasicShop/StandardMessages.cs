using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicShop
{
    public class StandardMessages
    {
        public static void Error(string message)
        {
            MessageBox.Show("Wystąpił błąd!\n" + message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
