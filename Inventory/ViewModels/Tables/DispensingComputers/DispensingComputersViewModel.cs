namespace Inventory.ViewModels.Tables.DispensingComputers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.DispensingComputers;
    using Inventory.View.Edit.Tables.DispensingComputers;
    using Inventory.ViewModels.Edit.Tables.DispensingComputers;
    using Inventory.ViewModels.Tables.Base;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DispensingComputersViewModel : BaseViewModel<Dispensing_computers>
    {
        public DispensingComputersViewModel() : base(DispensingComputers) => RefreshCollection();

        public static ObservableCollection<Dispensing_computers> DispensingComputers { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Дата выдачи":
                        {
                            DispensingComputers.Sort(data => data.Date_dispensing, SortDirection);
                            break;
                        }
                    case "Выдал":
                        {
                            DispensingComputers.Sort(user => user.User.Employee.L_name, SortDirection);
                            break;
                        }
                    case "Получил":
                        {
                            DispensingComputers.Sort(employee => employee.Employee.L_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddDispensingComputerCommand => new DelegateCommand(() =>
        {
            var addWindow = new DispensingComputerAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditDispensingComputerCommand => new DelegateCommand<Dispensing_computers>(selectDispensing =>
        {
            var editWindow = new DispensingComputerEditWindow();
            var viewModel = new DispensingComputerEditViewModel(selectDispensing);
            editWindow.DataContext = viewModel;
            editWindow.Closing += viewModel.OnWindowClosing;
            editWindow.ShowDialog();
            RefreshCollection();
        }, selectDispensing => selectDispensing != null);

        public ICommand DeleteDispensingComputerCommand => new DelegateCommand<Dispensing_computers>(selectDispensing =>
        {
            var messageResult = MessageBox.Show(
                $"Вы действительно хотите удалить выдачу:\nВыдал - {selectDispensing.User.Employee.L_name} {selectDispensing.User.Employee.F_name} {selectDispensing.User.Employee.M_name};\nПолучил - {selectDispensing.Employee.L_name} {selectDispensing.Employee.F_name} {selectDispensing.Employee.M_name};\nДата выдачи - {selectDispensing.Date_dispensing.Date:dd.MM.yyyy}?",
                "Удаление выдачи компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Dispensing_computers>(selectDispensing.Id_dispensing_computer))
                DispensingComputers.Remove(selectDispensing);
        }, selectDispensing => selectDispensing != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        public static void RefreshCollection()
        {
            DispensingComputers.Clear();

            using var db = new InventoryEntities();

            foreach (var item in db.Dispensing_computers
                .Include(list => list.List_dispensed_computers.Select(dispensed => dispensed.Dispensing_computers))
                .Include(list => list.List_dispensed_computers.Select(graphicsCards => graphicsCards.Computer.Inventory_numbers_graphics_cards.Graphics_cards.Manufacturer))
                .Include(list => list.List_dispensed_computers.Select(graphicsCards => graphicsCards.Computer.Inventory_numbers_graphics_cards.Graphics_cards.Unit))
                .Include(list => list.List_dispensed_computers.Select(hddComp => hddComp.Computer.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Manufacturer)))
                .Include(list => list.List_dispensed_computers.Select(hddComp => hddComp.Computer.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Types_hdd)))
                .Include(list => list.List_dispensed_computers.Select(hddComp => hddComp.Computer.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Unit)))
                .Include(list => list.List_dispensed_computers.Select(hddComp => hddComp.Computer.Hdd_in_computers.Select(inventoryHdd => inventoryHdd.Inventory_numbers_hdd.Hdd.Unit)))
                .Include(list => list.List_dispensed_computers.Select(ramComp => ramComp.Computer.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Manufacturer)))
                .Include(list => list.List_dispensed_computers.Select(ramComp => ramComp.Computer.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Types_memory)))
                .Include(list => list.List_dispensed_computers.Select(ramComp => ramComp.Computer.Ram_in_computers.Select(inventoryRam => inventoryRam.Inventory_numbers_ram.Ram.Unit)))
                .Include(list => list.List_dispensed_computers.Select(ssdComp => ssdComp.Computer.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Manufacturer)))
                .Include(list => list.List_dispensed_computers.Select(ssdComp => ssdComp.Computer.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Types_ssd)))
                .Include(list => list.List_dispensed_computers.Select(ssdComp => ssdComp.Computer.Ssd_in_computers.Select(inventorySsd => inventorySsd.Inventory_numbers_ssd.Ssd.Manufacturer)))
                .Include(list => list.List_dispensed_computers.Select(procComp => procComp.Computer.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Manufacturer)))
                .Include(list => list.List_dispensed_computers.Select(procComp => procComp.Computer.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Socket)))
                .Include(list => list.List_dispensed_computers.Select(procComp => procComp.Computer.Processors_in_computers.Select(inventoryProc => inventoryProc.Inventory_numbers_processors.Processor.Unit)))
                .Include(list => list.List_dispensed_computers.Select(status => status.Computer.Statuses_computers))
                .Include(list => list.List_dispensed_computers.Select(motherboard => motherboard.Computer.Inventory_numbers_motherboards.Motherboard.Manufacturer))
                .Include(list => list.List_dispensed_computers.Select(motherboard => motherboard.Computer.Inventory_numbers_motherboards.Motherboard.Socket))
                .Include(list => list.List_dispensed_computers.Select(status => status.Computer.Statuses_computers))
                .Include(list => list.List_dispensed_computers.Select(status => status.Computer.Statuses_computers))
                .Include(list => list.List_dispensed_computers.Select(power => power.Computer.Inventory_numbers_power_supplies.Power_supplies.Manufacturer))
                .Include(list => list.List_dispensed_computers.Select(power => power.Computer.Inventory_numbers_power_supplies.Power_supplies.Unit))
                .Include(list => list.List_dispensed_computers.Select(systemComp => systemComp.Computer.Operating_systems_in_computers.Select(system => system.Operating_systems)))
                .Include(employee => employee.Employee.Posts_employees.Select(post => post.Post))
                .Include(employee => employee.Employee.Employees_in_departments.Select(department => department.Department))
                .Include(user => user.User.Employee.Posts_employees.Select(post => post.Post))
                .Include(user => user.User.Employee.Employees_in_departments.Select(department => department.Department)))
            {
                DispensingComputers.Add(item);
            }

            DispensingComputers.Sort(dispensingComputers => dispensingComputers.Date_dispensing);
        }
    }
}