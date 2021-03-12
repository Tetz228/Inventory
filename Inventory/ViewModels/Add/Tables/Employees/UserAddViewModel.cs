﻿namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class UserAddViewModel : BindableBase
    {
        public UserAddViewModel()
        {
            User = new User();
            using var db = new InventoryEntities();
            Employees = new ObservableCollection<Employee>(db.Employees);
        }

        public User User { get; }

        public ObservableCollection<Employee> Employees { get; set; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            User.AddUser(User);

            addWindow.Close();

        }, _ => User.IsValidationProperties() && User.ValidPassword() && User.Fk_employee != 0 && User.EqualsPasswords());

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

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
