namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.Employees;

    public class UsersViewModel : BindableBase
    {
        public UsersViewModel()
        {
            using var db = new InventoryEntities();
            Users = new ObservableCollection<User>(db.Users.Include(employee => employee.Employee));

            UsersCollection = CollectionViewSource.GetDefaultView(Users);
            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Login), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<User> Users { get; set; }

        public ICollectionView UsersCollection { get; }

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

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Логин":
                    {
                        if (UsersCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Login),
                                ListSortDirection.Descending));
                        }
                        else
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Login),
                                ListSortDirection.Ascending));
                        }

                        UsersCollection.Refresh();

                        break;
                    }
                case "ФИО":
                    {
                        if (UsersCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Employee.L_name),
                                ListSortDirection.Descending));
                        }
                        else
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Employee.L_name),
                                ListSortDirection.Ascending));
                        }

                        UsersCollection.Refresh();

                        break;
                    }
                case "Почта":
                    {
                        if (UsersCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Employee.Email),
                                ListSortDirection.Descending));
                        }
                        else
                        {
                            UsersCollection.SortDescriptions.Clear();
                            UsersCollection.SortDescriptions.Add(new SortDescription(nameof(User.Employee.Email),
                                ListSortDirection.Ascending));
                        }

                        UsersCollection.Refresh();

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