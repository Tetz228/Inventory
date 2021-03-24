namespace Inventory.ViewModels.Recovery__password
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Pages.Recovery_password;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
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
                        else if (Employee.IsValidationEmail(Email) == false)
                            result = "Некорректная почта";
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

        public string Email { get; set; }

        public ICommand FurtherCommand => new DelegateCommand(() =>
        {
            var securityСodePage = new SecurityСodePage();
            var securityСodeViewModel = new SecurityСodeViewModel(Email,1111, 2);

            securityСodePage.DataContext = securityСodeViewModel;
            PasswordRecoveryViewModel.PageNavigation.Navigate(securityСodePage);

            //(Employee employee, bool existEmployee) = Employee.OnEmailExist(Email);

            //if (existEmployee)
            //{
            //    (int idUser, bool existUser) = User.OnUserExist(employee);

            //    if (existUser)
            //    {
            //        (string code, bool codeSent) = Employee.SendingSecurityCode(Email);

            //        if (codeSent)
            //        {
            //            var securityСodePage = new SecurityСodePage();
            //            var securityСodeViewModel = new SecurityСodeViewModel(code, idUser);

            //            securityСodePage.DataContext = securityСodeViewModel;
            //            PasswordRecoveryViewModel.PageNavigation.Navigate(securityСodePage);
            //        }
            //    }
            //}

        }, IsValidationProperties);

        public ICommand CancelCommand => new DelegateCommand(() => PasswordRecoveryViewModel.RecoveryWindow.Close());
    }
}
