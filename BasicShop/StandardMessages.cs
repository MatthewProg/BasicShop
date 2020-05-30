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
