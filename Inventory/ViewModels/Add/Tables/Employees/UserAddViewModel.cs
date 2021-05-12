namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class UserAddViewModel : BindableBase
    {
        public UserAddViewModel()
        {
            using var db = new InventoryEntities();

            Employees = new ObservableCollection<Employee>(db.Employees.AsNoTracking()).Sort(emp => emp.L_name);
            Roles = new ObservableCollection<Role>(db.Roles.AsNoTracking()).Sort(role => role.Name);
        }

        #region Свойства
        public User User { get; } = new();

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<Role> Roles { get; }
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
                            && User.Password?.Length > 2
                            && User.Password.Equals(User.PasswordRepeated));

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
        
        #endregion
    }
}