namespace Inventory.ViewModels.RecoveryPassword
{
    using System.Windows;
    using System.Windows.Controls;

    using DevExpress.Mvvm;

    using Inventory.Services;
    using Inventory.View.Pages.RecoveryPassword;

    public class PasswordRecoveryViewModel : BindableBase
    {
        public PasswordRecoveryViewModel()
        {
            PageNavigation = new PageNavigation();
            PageNavigation.OnPageChanged += page => CurrentPage = page;
            PageNavigation.Navigate(new EmailPage());
        }

        public void Window_Loaded(object sender, RoutedEventArgs args) => RecoveryWindow = (Window)sender;

        public static Window RecoveryWindow { get; set; }

        public static PageNavigation PageNavigation { get; set; }

        public Page CurrentPage { get; private set; }
    }
}
