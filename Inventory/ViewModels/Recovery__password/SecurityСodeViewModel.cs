namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model;
    using Inventory.View.Pages.Recovery_password;

    public class SecurityСodeViewModel : BindableBase, IDataErrorInfo
    {
        public SecurityСodeViewModel(string email, int sentСode, int idUser)
        {
            Email = email;
            SentСode = sentСode;
            IdUser = idUser;
        }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "EnteredCode":
                        if (EnteredCode.ToString().Length < 4)
                            result = "Код должен состоять из 4 символов";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);
        #endregion

        public int EnteredCode { get; set; }

        private string Email { get; }

        private int SentСode { get; set; }

        private int IdUser { get; }

        public ICommand FurtherCommand => new DelegateCommand(() =>
        {
            if (SentСode.Equals(EnteredCode))
            {
                var newPasswordPage = new NewPasswordPage();
                var newPasswordViewModel = new NewPasswordViewModel(IdUser);

                newPasswordPage.DataContext = newPasswordViewModel;
                PasswordRecoveryViewModel.PageNavigation.Navigate(newPasswordPage);
            }
            else
                MessageBox.Show("Ошибка! Введенный код не соответствует отправленному.", "Ошибка при проверки кода безопасности.", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }, IsValidationProperties);

        public ICommand SendingSecurityCodeCommand => new DelegateCommand(() =>
        {
            (int code, bool codeSent) = Employee.SendingSecurityCode(Email);

            if (codeSent)
                SentСode = code;
        }, true);

        public ICommand CancelCommand => new DelegateCommand(() => PasswordRecoveryViewModel.RecoveryWindow.Close());
    }
}
