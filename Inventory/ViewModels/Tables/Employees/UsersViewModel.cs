namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;

    public class UsersViewModel : BindableBase
    {
        public UsersViewModel()
        {
            using var db = new InventoryEntities();

            Users = new ObservableCollection<User>(db.Users.AsNoTracking().Include(employee => employee.Employee).Include(role => role.Role));
            Users.Sort(user => user.Login, SortDirection = ListSortDirection.Ascending);
            UsersCollection = CollectionViewSource.GetDefaultView(Users);
        }

        #region Свойства

        private ICollectionView UsersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<User> Users { get; set; }

        public User SelectUser { get; set; }

        private string _usersFilter = string.Empty;

        public string UsersFilter
        {
            get => _usersFilter;
            set
            {
                _usersFilter = value;
                UsersCollection.Filter = obj =>
                {
                    if (obj is User user)
                        return user.Search(UsersFilter);

                    return false;
                };
                UsersCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectUser = null;
        #endregion

        #region Команды
        public ICommand AddUserCommand => new DelegateCommand(() =>
        {
            var addUserWindow = new UserAddWindow();
            addUserWindow.ShowDialog();
        });

        public ICommand EditUserCommand => new DelegateCommand<User>((user) =>
        {
            var editWindow = new UserEditWindow();
            var viewModel = new UserEditViewModel(user);
            editWindow.DataContext = viewModel;
            editWindow.Closing += viewModel.OnWindowClosing;
            editWindow.ShowDialog();
        });

        public ICommand DeleteUserCommand => new DelegateCommand<User>(selectUser =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectUser.Login}?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<User>(selectUser.Id_user);
            RefreshCollection();
        }, selectUser => selectUser != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Users.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Users.AsNoTracking().Include(employee => employee.Employee).Include(role => role.Role))
                Users.Add(item);
        }
    }
}