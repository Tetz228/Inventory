namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.View.Pages.Recovery_password;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    public class EmailViewModel : BindableBase, IDataErrorInfo
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
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Поле не должно быть пустым";
                        else if (!MailsInteraction.IsValidationEmail(Email))
                            result = "Некорректная почта";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }
        #endregion

        public string Email { get; set; }

        public ICommand FurtherCommand => new DelegateCommand(() =>
        {
            (Employee employee, bool existEmployee) = MailsInteraction.OnEmailExist(Email);

            if (existEmployee)
            {
                (int idUser, bool existUser) = UsersInteraction.OnUserExist(employee);

                if (existUser)
                {
                    (int code, bool codeSent) = MailsInteraction.SendingSecurityCode(Email);

                    if (codeSent)
                    {
                        var securityСodePage = new SecurityСodePage();
                        var securityСodeViewModel = new SecurityСodeViewModel(Email, code, idUser);

                        securityСodePage.DataContext = securityСodeViewModel;
                        PasswordRecoveryViewModel.PageNavigation.Navigate(securityСodePage);
                    }
                }
            }

        }, Services.IsValidationProperties(ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand(() => PasswordRecoveryViewModel.RecoveryWindow.Close());
    }
}
