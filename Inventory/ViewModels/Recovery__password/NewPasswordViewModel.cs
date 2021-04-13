namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class NewPasswordViewModel : BindableBase
    {
        public NewPasswordViewModel(int idUser)
        {
            User = new User { Id_user = idUser };
        }

        private User User { get; }

        #region Команды
        public ICommand EditCommand => new DelegateCommand(() =>
        {
            UsersInteraction.ChangePassword(User);
            PasswordRecoveryViewModel.RecoveryWindow.Close();
        }, () => User.Password?.Length > 2 && User.Password.Equals(User.PasswordRepeated));

        public ICommand PasswordChanged => new DelegateCommand<PasswordBox>(passwordBox =>
        {
            if (passwordBox != null)
                User.Password = passwordBox.Password;
        }, _ => true);

        public ICommand PasswordChangedRepeated => new DelegateCommand<PasswordBox>(passwordBox =>
        {
            if (passwordBox != null)
                User.PasswordRepeated = passwordBox.Password;
        }, _ => true);

        public ICommand CancelCommand => new DelegateCommand(() =>
        {
            if (PasswordRecoveryViewModel.RecoveryWindow != null)
                PasswordRecoveryViewModel.RecoveryWindow.Close();
        });
        #endregion
    }
}
