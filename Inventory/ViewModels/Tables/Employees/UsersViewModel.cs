namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class UsersViewModel : BaseViewModel<User>
    {
        public UsersViewModel() : base(Users) => RefreshCollection();

        public static ObservableCollection<User> Users { get; set; } = new();
        
        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Логин":
                        {
                            Users.Sort(user => user.Login, SortDirection);
                            break;
                        }
                    case "ФИО":
                        {
                            Users.Sort(user => user.Employee.L_name, SortDirection);
                            break;
                        }
                    case "Почта":
                        {
                            Users.Sort(user => user.Employee.Email, SortDirection);
                            break;
                        }
                    case "Роль":
                        {
                            Users.Sort(role => role.Role.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddUserCommand => new DelegateCommand(() =>
        {
            var addUserWindow = new UserAddWindow();
            addUserWindow.ShowDialog();
        });

        public ICommand EditUserCommand => new DelegateCommand<User>(user =>
        {
            var editWindow = new UserEditWindow();
            var viewModel = new UserEditViewModel(user);
            editWindow.DataContext = viewModel;
            editWindow.Closing += viewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, selectUser => selectUser != null);

        public ICommand DeleteUserCommand => new DelegateCommand<User>(selectUser =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить пользователя:\nлогин - {selectUser.Login};\nроль - {selectUser.Role.Name}?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<User>(selectUser.Id_user))
                Users.Remove(selectUser);
        }, selectUser => selectUser != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        public static void RefreshCollection()
        {
            Users.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Users.AsNoTracking().Include(employee => employee.Employee).Include(role => role.Role))
            {
                Users.Add(item);
            }
            Users.Sort(user => user.Login);
        }
    }
}