namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Services;

    public class UserAddViewModel : BindableBase
    {
        public UserAddViewModel()
        {
            User = new User();
            using var db = new InventoryEntities();
            Employees = new ObservableCollection<Employee>(db.Employees);
        }

        #region Свойства
        public User User { get; }

        public ObservableCollection<Employee> Employees { get; }
        #endregion

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            (string salt, string hash) = UsersInteraction.GenerateSaltAndHashingPassword(User.Password);

            User.Salt = salt;
            User.Password = hash;

            Services.Add(User);
            UsersViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(User.ErrorCollection)
                            && UsersInteraction.ValidPassword(User.Password)
                            && UsersInteraction.EqualsPasswords(User.Password, User.PasswordRepeated));

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

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}