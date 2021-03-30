namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Services;

    public class AuthorizationViewModel : BindableBase, IDataErrorInfo
    {
        #region Валидация

        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Login":
                        if (string.IsNullOrWhiteSpace(Login))
                            result = "Поле не должно быть пустым";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        #endregion

        public string Login { get; set; }

        private string Password { get; set; }

        public ICommand ComeInCommand => new DelegateCommand<Window>(authWindow =>
        {
            var authorizedUser = UsersInteraction.OnUserExist(Login, Password);

            if (authorizedUser != null)
            {
                User.AuthorizedUser = authorizedUser;

                var mainWindow = new MainWindow();
                mainWindow.Show();

                authWindow.Close();
            }
        }, _ => Services.IsValidationProperties(ErrorCollection));


        public ICommand PasswordRecoveryCommand => new DelegateCommand(() =>
        {
            var passwordRecoveryWindow = new PasswordRecoveryWindow();
            passwordRecoveryWindow.ShowDialog();
        });

        public ICommand PasswordChanged => new DelegateCommand<PasswordBox>(passwordBox =>
        {
            if (passwordBox != null)
                Password = passwordBox.Password;
        }, _ => true);
    }
}
