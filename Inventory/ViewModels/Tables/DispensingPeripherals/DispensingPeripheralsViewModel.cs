namespace Inventory.ViewModels.Tables.DispensingPeripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.DispensingPeripherals;
    using Inventory.View.Edit.Tables.DispensingPeripherals;
    using Inventory.ViewModels.Edit.Tables.DispensingPeripherals;
    using Inventory.ViewModels.Tables.Base;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DispensingPeripheralsViewModel : BaseViewModel<Dispensing_peripherals>
    {
        public DispensingPeripheralsViewModel() : base(DispensingPeripherals) => RefreshCollection();

        #region Свойства

        public static ObservableCollection<Dispensing_peripherals> DispensingPeripherals { get; set; } = new();

        private string _dispensingFilter = string.Empty;

        public string DispensingFilter
        {
            get => _dispensingFilter;
            set
            {
                _dispensingFilter = value;
                CollectionView.Filter = obj =>
                {
                    if (obj is Dispensing_peripherals dispensing)
                        return dispensing.Search(DispensingFilter);

                    return false;
                };
                CollectionView.Refresh();
            }
        }
        #endregion

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Дата выдачи":
                        {
                            DispensingPeripherals.Sort(data => data.Date_dispensing, SortDirection);
                            break;
                        }
                    case "Выдал":
                        {
                            DispensingPeripherals.Sort(user => user.User.Employee.L_name, SortDirection);
                            break;
                        }
                    case "Получил":
                        {
                            DispensingPeripherals.Sort(employee => employee.Employee.L_name, SortDirection);
                            break;
                        }
                }
            }
        }

        #region Команды
        public ICommand AddDispensingPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new DispensingPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditDispensingPeripheralCommand => new DelegateCommand<Dispensing_peripherals>(selectDispensing =>
        {
            var editWindow = new DispensingPeripheralEditWindow();
            var viewModel = new DispensingPeripheralEditViewModel(selectDispensing);
            editWindow.DataContext = viewModel;
            editWindow.Closing += viewModel.OnWindowClosing;
            editWindow.ShowDialog();
            RefreshCollection();
        }, selectDispensing => selectDispensing != null);

        public ICommand DeleteDispensingPeripheralCommand => new DelegateCommand<Dispensing_peripherals>(selectDispensing =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить выдачу:\nВыдал - {selectDispensing.User.Employee.L_name} {selectDispensing.User.Employee.F_name} {selectDispensing.User.Employee.M_name};\nПолучил - {selectDispensing.Employee.L_name} {selectDispensing.Employee.F_name} {selectDispensing.Employee.M_name};\nДата выдачи - {selectDispensing.Date_dispensing.Date:dd.MM.yyyy}?", "Удаление выдачи периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Dispensing_peripherals>(selectDispensing.Id_dispensing_peripheral))
                DispensingPeripherals.Remove(selectDispensing);
        }, selectDispensing => selectDispensing != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            DispensingPeripherals.Clear();

            using var db = new InventoryEntities();

            foreach (var item in db.Dispensing_peripherals
                .Include(list => list.List_dispensed_peripherals.Select(dispensed => dispensed.Dispensing_peripherals))
                .Include(list => list.List_dispensed_peripherals.Select(manufacturer => manufacturer.Inventory_numbers_peripherals.Peripheral.Manufacturer))
                .Include(list => list.List_dispensed_peripherals.Select(type => type.Inventory_numbers_peripherals.Peripheral.Types_peripherals))
                .Include(list => list.List_dispensed_peripherals.Select(type => type.Inventory_numbers_peripherals.Statuses_peripherals))
                    .Include(employee => employee.Employee.Posts_employees.Select(post => post.Post))
                    .Include(employee => employee.Employee.Employees_in_departments.Select(department => department.Department))
                    .Include(user => user.User.Employee.Posts_employees.Select(post => post.Post))
                    .Include(user => user.User.Employee.Employees_in_departments.Select(department => department.Department)))
            {
                DispensingPeripherals.Add(item);
            }
            DispensingPeripherals.Sort(dispensingPeripherals => dispensingPeripherals.Date_dispensing);
        }
    }
}
