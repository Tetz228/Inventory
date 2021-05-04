namespace Inventory.ViewModels.Edit.Tables.DispensingPeripherals
{
    using DevExpress.Mvvm;

    using Inventory.Model;

    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class DispensingPeripheralEditViewModel : BindableBase
    {
        public DispensingPeripheralEditViewModel(Dispensing_peripherals dispensingPeripherals)
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Inventory_numbers_peripherals>(db.Inventory_numbers_peripherals.AsNoTracking()
                    .Include(status => status.Statuses_peripherals)
                    .Include(peripheral => peripheral.Peripheral)
                    .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                    .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals))
                .Sort(numberPeripheral => numberPeripheral.Inventory_number);
            Employees = new ObservableCollection<Employee>(db.Employees.AsNoTracking()).Sort(emp => emp.L_name);
            ListDispensedPeripherals = new ObservableCollection<Inventory_numbers_peripherals>(dispensingPeripherals.List_dispensed_peripherals.Select(peripheral => peripheral.Inventory_numbers_peripherals)).Sort(inventoryNumbers => inventoryNumbers.Inventory_number);
            DispensingPeripherals = dispensingPeripherals;
            DispensingPeripherals.BeginEdit();

            for (int i = Peripherals.Count - 1; i >= 0; i--)
            {
                foreach (var peripherals in db.List_dispensed_peripherals.AsNoTracking())
                {
                    if (Peripherals[i].Id_inventory_number_peripheral == peripherals.Fk_inventory_number_peripheral)
                    {
                        Peripherals.Remove(Peripherals[i]);
                        break;
                    }
                }
            }
        }

        #region Свойства

        public Inventory_numbers_peripherals SelectInventoryPeripheral { get; set; }

        public Inventory_numbers_peripherals SelectListPeripheral { get; set; }

        public Dispensing_peripherals DispensingPeripherals { get; set; }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<Inventory_numbers_peripherals> Peripherals { get; set; }

        public ObservableCollection<Inventory_numbers_peripherals> ListDispensedPeripherals { get; set; }

        #endregion

        public void OnMouseLeftButtonDownListPeripherals(object sender, RoutedEventArgs args) => SelectInventoryPeripheral = null;

        public void OnMouseLeftButtonDownListDispensing(object sender, RoutedEventArgs args) => SelectListPeripheral = null;

        public void OnWindowClosing(object sender, CancelEventArgs e) => DispensingPeripherals.CancelEdit();

        public List_dispensed_peripherals GetListDispensedPeripherals(int id)
        {
            using var db = new InventoryEntities();

            return db.List_dispensed_peripherals.FirstOrDefault(peripheral => peripheral.Fk_inventory_number_peripheral == id);
        }

        #region Команды

        public ICommand TransferInListDispensingCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            ListDispensedPeripherals.Add(selectPeripheral);
            Peripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand TransferInListInventoryPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            var listDispensedPeripherals = GetListDispensedPeripherals(selectPeripheral.Id_inventory_number_peripheral);

            if (listDispensedPeripherals != null)
            {
                var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарную периферию:\nинвент. номер - {selectPeripheral.Inventory_number};\nпроизводитель - {selectPeripheral.Peripheral.Manufacturer.Name};\nтип - {selectPeripheral.Peripheral.Types_peripherals.Name};\nнаименование - {selectPeripheral.Peripheral.Name}?\nЭто действие невозможно отменить.", "Удаление инвентарной периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageResult != MessageBoxResult.Yes)
                    return;

                Services.Delete<List_dispensed_peripherals>(listDispensedPeripherals.Id_list_dispensed_peripheral);
            }

            Peripherals.Add(selectPeripheral);
            ListDispensedPeripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand EditCommand => new DelegateCommand<Window>(addWindow =>
        {
            DispensingPeripherals.EndEdit();

            Services.Edit(DispensingPeripherals.Id_dispensing_peripheral,DispensingPeripherals);

            foreach (var item in ListDispensedPeripherals)
            {
                if (GetListDispensedPeripherals(item.Id_inventory_number_peripheral) == null)
                {
                    var newListDispensed = new List_dispensed_peripherals
                    {
                        Fk_number_dispensing = DispensingPeripherals.Id_dispensing_peripheral,
                        Fk_inventory_number_peripheral = item.Id_inventory_number_peripheral
                    };
                    Services.Add(newListDispensed);
                }
            }
            addWindow.Close();
        }, _ => ListDispensedPeripherals?.Count > 0);

        #endregion
    }
}
