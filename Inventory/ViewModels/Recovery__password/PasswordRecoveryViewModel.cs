namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using Inventory.Services;
    using Inventory.View.Pages.Recovery_password;
    using System.Windows;
    using System.Windows.Controls;

    public class PasswordRecoveryViewModel : BindableBase
    {
        public PasswordRecoveryViewModel()
        {
            PageNavigation = new PageNavigation();
            PageNavigation.OnPangeChanged += page => CurrentPage = page;
            PageNavigation.Navigate(new EmailPage());
        }

        public void Window_Loaded(object sender, RoutedEventArgs args) => RecoveryWindow = (Window)sender;

        public static Window RecoveryWindow { get; set; }

        public static PageNavigation PageNavigation { get; set; }

        public Page CurrentPage { get; private set; }
    }
}
