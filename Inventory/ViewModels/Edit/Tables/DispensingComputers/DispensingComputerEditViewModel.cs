namespace Inventory.ViewModels.Edit.Tables.DispensingComputers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DispensingComputerEditViewModel : BindableBase
    {
        public DispensingComputerEditViewModel(Dispensing_computers dispensingComputers)
        {
            using var db = new InventoryEntities();

            Computers = new ObservableCollection<Computer>(db.Computers.AsNoTracking()
                .Include(status => status.Statuses_computers)
                .Include(graphicsCards => graphicsCards.Inventory_numbers_graphics_cards.Graphics_cards.Manufacturer)
                .Include(graphicsCards => graphicsCards.Inventory_numbers_graphics_cards.Graphics_cards.Unit)
                .Include(hddComp => hddComp.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Manufacturer))
                .Include(hddComp => hddComp.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Types_hdd))
                .Include(hddComp => hddComp.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Unit))
                .Include(ramComp => ramComp.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Manufacturer))
                .Include(ramComp => ramComp.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Types_memory))
                .Include(ramComp => ramComp.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Unit))
                .Include(ssdComp => ssdComp.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Manufacturer))
                .Include(ssdComp => ssdComp.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Types_ssd))
                .Include(ssdComp => ssdComp.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Unit))
                .Include(procComp => procComp.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Manufacturer))
                .Include(procComp => procComp.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Socket))
                .Include(procComp => procComp.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Unit))
                .Include(motherboard => motherboard.Inventory_numbers_motherboards.Motherboard.Manufacturer)
                .Include(motherboard => motherboard.Inventory_numbers_motherboards.Motherboard.Socket)
                .Include(power => power.Inventory_numbers_power_supplies.Power_supplies.Manufacturer)
                .Include(power => power.Inventory_numbers_power_supplies.Power_supplies.Unit)
                .Include(systemComp => systemComp.Operating_systems_in_computers.Select(system => system.Operating_systems)))
                .Sort(number => number.Inventory_number);
            Employees = new ObservableCollection<Employee>(db.Employees.AsNoTracking()).Sort(emp => emp.L_name);
            ListDispensedComputers = new ObservableCollection<Computer>(dispensingComputers.List_dispensed_computers.Select(computer => computer.Computer)).Sort(inventoryNumbers => inventoryNumbers.Inventory_number);

            DispensingComputers = dispensingComputers;
            DispensingComputers.BeginEdit();

            for (int i = Computers.Count - 1; i >= 0; i--)
            {
                foreach (var computers in db.List_dispensed_computers.AsNoTracking())
                {
                    if (Computers[i].Id_computer == computers.Fk_computer)
                    {
                        Computers.Remove(Computers[i]);
                        break;
                    }
                }
            }
        }

        #region Свойства

        private ListSortDirection SortDirection { get; set; }

        public Computer SelectComputer { get; set; }

        public Computer SelectListComputer { get; set; }

        public Dispensing_computers DispensingComputers { get; set; }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<Computer> Computers { get; set; }

        public ObservableCollection<Computer> ListDispensedComputers { get; set; }

        #endregion

        #region События

        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Computers.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Descending);
                            else
                                Computers.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDownListComputers(object sender, RoutedEventArgs args) => SelectComputer = null;

        public void OnMouseLeftButtonDownListDispensing(object sender, RoutedEventArgs args) => SelectListComputer = null;

        public void OnWindowClosing(object sender, CancelEventArgs e) => DispensingComputers.CancelEdit();

        #endregion

        private List_dispensed_computers GetDispensedComputer(int id)
        {
            using var db = new InventoryEntities();

            return db.List_dispensed_computers.FirstOrDefault(computer => computer.Fk_computer == id);
        }

        #region Команды

        public ICommand TransferInListDispensingCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            ListDispensedComputers.Add(selectComputer);
            Computers.Remove(selectComputer);
        }, selectComputer => selectComputer != null);

        public ICommand TransferInListInventoryPeripheralCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            var dispensedComputer = GetDispensedComputer(selectComputer.Id_computer);

            if (dispensedComputer != null)
            {
                var messageResult = MessageBox.Show($"Вы действительно хотите удалить компьютер:\nинверт. номер - {selectComputer.Inventory_number};\nIP-адрес - {selectComputer.Ip_address};\nСтатус - {selectComputer.Statuses_computers.Name}?\nЭто действие невозможно отменить.", "Удаление компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageResult != MessageBoxResult.Yes)
                    return;

                Services.Delete<List_dispensed_computers>(dispensedComputer.Id_list_dispensed_computer);
            }

            Computers.Add(selectComputer);
            ListDispensedComputers.Remove(selectComputer);
        }, selectComputer => selectComputer != null);

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            DispensingComputers.EndEdit();

            Services.Edit(DispensingComputers.Id_dispensing_computer, DispensingComputers);

            foreach (var item in ListDispensedComputers)
            {
                if (GetDispensedComputer(item.Id_computer) == null)
                {
                    var newListDispensed = new List_dispensed_computers
                    {
                        Fk_number_dispensing = DispensingComputers.Id_dispensing_computer,
                        Fk_computer = item.Id_computer
                    };
                    Services.Add(newListDispensed);
                }
            }
            editWindow.Close();
        }, _ => ListDispensedComputers?.Count > 0);

        #endregion
    }
}
