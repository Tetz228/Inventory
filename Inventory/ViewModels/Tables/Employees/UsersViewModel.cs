namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class UsersViewModel : BindableBase
    {
        public UsersViewModel()
        {
            using var db = new InventoryEntities();

            Users = new ObservableCollection<User>(db.Users.Include(employee => employee.Employee));
            Users.Sort(user => user.Login, SortDirection = ListSortDirection.Ascending);
            UsersCollection = CollectionViewSource.GetDefaultView(Users);
        }

        #region Свойства

        private ICollectionView UsersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<User> Users { get; set; }

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
                        return User.Search(user, UsersFilter);

                    return false;
                };
                UsersCollection.Refresh();
            }
        }
        #endregion

        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Логин":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Users.Sort(user => user.Login, SortDirection = ListSortDirection.Descending);
                    else
                        Users.Sort(user => user.Login, SortDirection = ListSortDirection.Ascending);
                    break;
                }
                case "ФИО":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Users.Sort(user => user.Employee.L_name, SortDirection = ListSortDirection.Descending);
                    else
                        Users.Sort(user => user.Employee.L_name, SortDirection = ListSortDirection.Ascending);
                    break;
                }
                case "Почта":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Users.Sort(user => user.Employee.Email, SortDirection = ListSortDirection.Descending);
                    else
                        Users.Sort(user => user.Employee.Email, SortDirection = ListSortDirection.Ascending);
                    break;
                }
            }
        }

        #region Команды
        public ICommand AddUser => new DelegateCommand(() =>
        {
            var addUserWindow = new UserAddWindow();

            addUserWindow.ShowDialog();
        });

        public ICommand RefreshList => new DelegateCommand(User.RefreshCollection);
        #endregion
    }
}