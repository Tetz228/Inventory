﻿namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class NewPasswordViewModel : BindableBase
    {
        public NewPasswordViewModel(int idUser)
        {
            User = new User { Id_user = idUser };
        }

        public User User { get; }

        #region Команды
        public ICommand EditCommand => new DelegateCommand(() =>
        {
            User.ChangePassword(User);
            PasswordRecoveryViewModel.RecoveryWindow.Close();
        }, () => User.ValidPassword() && User.EqualsPasswords());

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

        public ICommand CancelCommand => new DelegateCommand(PasswordRecoveryViewModel.RecoveryWindow.Close);
        #endregion
    }
}
