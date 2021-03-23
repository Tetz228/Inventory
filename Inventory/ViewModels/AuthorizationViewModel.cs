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

    public class AuthorizationViewModel : BindableBase, IDataErrorInfo
    {
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

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);

        public string Login { get; set; }

        public string Password { get; set; }

        public ICommand ComeInCommand => new DelegateCommand<Window>(authWindow =>
        {
            var (idUser, userExist) = User.OnUserExist(Login, Password);

            if (userExist)
            {
                User.AuthorizedUser = idUser;

                var mainWindow = new MainWindow();
                mainWindow.Show();

                authWindow.Close();
            }
        }, _ => IsValidationProperties() && Password?.Length > 2);

        public ICommand PasswordRecoveryCommand => new DelegateCommand(() =>
        {

        });

        public ICommand PasswordChanged => new DelegateCommand<PasswordBox>(passwordBox =>
        {
            if (passwordBox != null)
                Password = passwordBox.Password;
        }, _ => true);
    }
}
