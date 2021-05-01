namespace Inventory.ViewModels.Tables.DispensingPeripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.DispensingPeripherals;

    public class DispensingPeripheralsViewModel : BindableBase
    {
        public DispensingPeripheralsViewModel()
        {
            RefreshCollection();
            DispensingCollection = CollectionViewSource.GetDefaultView(DispensingPeripherals);
        }

        #region Свойства

        private ICollectionView DispensingCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Dispensing_peripherals> DispensingPeripherals { get; set; } = new();

        public Dispensing_peripherals SelectDispensing { get; set; }

        private string _dispensingFilter = string.Empty;

        public string DispensingFilter
        {
            get => _dispensingFilter;
            set
            {
                _dispensingFilter = value;
                DispensingCollection.Filter = obj =>
                {
                    if (obj is User dispensing)
                        return dispensing.Search(DispensingFilter);

                    return false;
                };
                DispensingCollection.Refresh();
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
                    //case "Логин":
                    //    {
                    //        DispensingPeripherals.Sort(user => user.Login, SortDirection);
                    //        break;
                    //    }
                    //case "ФИО":
                    //    {
                    //        DispensingPeripherals.Sort(user => user.Employee.L_name, SortDirection);
                    //        break;
                    //    }
                    //case "Почта":
                    //    {
                    //        DispensingPeripherals.Sort(user => user.Employee.Email, SortDirection);
                    //        break;
                    //    }
                    //case "Роль":
                    //    {
                    //        DispensingPeripherals.Sort(role => role.Role.Name, SortDirection);
                    //        break;
                    //    }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectDispensing = null;
        #endregion

        #region Команды
        public ICommand AddDispensingPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new DispensingPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditDispensingPeripheralCommand => new DelegateCommand<Dispensing_peripherals>(selectDispensing =>
        {
            //var editWindow = new UserEditWindow();
            //var viewModel = new UserEditViewModel(user);
            //editWindow.DataContext = viewModel;
            //editWindow.Closing += viewModel.OnWindowClosing;
            //editWindow.ShowDialog();
        }, selectDispensing => selectDispensing != null);

        public ICommand DeleteDispensingPeripheralCommand => new DelegateCommand<Dispensing_peripherals>(selectDispensing =>
        {
            //var messageResult = MessageBox.Show($"Вы действительно хотите удалить выдачу:\nВыдал - {selectDispensing.Login};\nПолучил - {selectDispensing.Role.Name}?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //if (messageResult != MessageBoxResult.Yes)
            //    return;

            Services.Delete<Dispensing_peripherals>(selectDispensing.Id_dispensing_peripheral);
            RefreshCollection();
        }, selectDispensing => selectDispensing != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            DispensingPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Dispensing_peripherals.AsNoTracking()
                    .Include(employee => employee.Employee.Posts_employees.Select(post => post.Post))
                    .Include(employee => employee.Employee.Employees_in_departments.Select(department => department.Department))
                    .Include(employee => employee.User.Employee.Posts_employees.Select(post => post.Post))
                    .Include(employee => employee.User.Employee.Employees_in_departments.Select(department => department.Department)))
            {
                DispensingPeripherals.Add(item);
            }
            DispensingPeripherals.Sort(dispensingPeripherals => dispensingPeripherals.Date_dispensing);
        }
    }
}
