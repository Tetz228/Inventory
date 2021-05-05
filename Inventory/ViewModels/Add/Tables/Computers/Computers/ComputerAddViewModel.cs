namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using Services;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class ComputerAddViewModel : BindableBase
    {
        public ComputerAddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryMotherboards = new ObservableCollection<Inventory_numbers_motherboards>(db.Inventory_numbers_motherboards
                    .Include(mother => mother.Motherboard.Socket)
                    .Include(mother => mother.Motherboard.Manufacturer))
                .Sort(number => number.Inventory_number);
            InventoryGraphicsCards = new ObservableCollection<Inventory_numbers_graphics_cards>(db.Inventory_numbers_graphics_cards
                    .Include(graphicsCards => graphicsCards.Graphics_cards.Manufacturer)
                    .Include(graphicsCards => graphicsCards.Graphics_cards.Unit))
                .Sort(number => number.Inventory_number);
            InventoryPowerSupplies = new ObservableCollection<Inventory_numbers_power_supplies>(db.Inventory_numbers_power_supplies
                    .Include(powerSupplies => powerSupplies.Power_supplies.Manufacturer)
                    .Include(powerSupplies => powerSupplies.Power_supplies.Unit))
                .Sort(number => number.Inventory_number);
            InventoryProcessors = new ObservableCollection<Inventory_numbers_processors>(db.Inventory_numbers_processors
                    .Include(unit => unit.Processor.Unit)
                    .Include(manufact => manufact.Processor.Manufacturer)
                    .Include(socket => socket.Processor.Socket))
                .Sort(number => number.Inventory_number);
            InventoryHdd = new ObservableCollection<Inventory_numbers_hdd>(db.Inventory_numbers_hdd
                    .Include(unit => unit.Hdd.Unit)
                    .Include(manufact => manufact.Hdd.Manufacturer)
                    .Include(socket => socket.Hdd.Types_hdd))
                .Sort(number => number.Inventory_number);
            InventoryRams = new ObservableCollection<Inventory_numbers_ram>(db.Inventory_numbers_ram
                    .Include(unit => unit.Ram.Unit)
                    .Include(manufact => manufact.Ram.Manufacturer)
                    .Include(type => type.Ram.Types_memory))
                .Sort(number => number.Inventory_number);
            InventorySsds = new ObservableCollection<Inventory_numbers_ssd>(db.Inventory_numbers_ssd
                    .Include(unit => unit.Ssd.Unit)
                    .Include(manufact => manufact.Ssd.Manufacturer)
                    .Include(socket => socket.Ssd.Types_ssd))
                .Sort(number => number.Inventory_number);

            StatusesComputers = new ObservableCollection<Statuses_computers>(db.Statuses_computers).Sort(status => status.Name);
            OperatingSystems = new ObservableCollection<Operating_systems>(db.Operating_systems).Sort(operatingSystems => operatingSystems.Name);

            try
            {
                Computer.Inventory_number = db.Computers.Select(computer => computer.Inventory_number).Max() + 1;
            }
            catch
            {
                Computer.Inventory_number = 1;
            }
        }

        #region Свойства

        public Computer Computer { get; } = new();

        public ObservableCollection<Inventory_numbers_motherboards> InventoryMotherboards { get; }

        public ObservableCollection<Inventory_numbers_graphics_cards> InventoryGraphicsCards { get; }

        public ObservableCollection<Inventory_numbers_power_supplies> InventoryPowerSupplies { get; }

        public ObservableCollection<Inventory_numbers_processors> InventoryProcessors { get; }

        public ObservableCollection<Inventory_numbers_ssd> InventorySsds { get; }

        public ObservableCollection<Inventory_numbers_ram> InventoryRams { get; }

        public ObservableCollection<Statuses_computers> StatusesComputers { get; }

        public ObservableCollection<Operating_systems> OperatingSystems { get; }

        public ObservableCollection<Inventory_numbers_hdd> InventoryHdd { get; }

        #endregion

        #region Команды
        public ICommand AddComputerCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Services.Add(Computer);
            ComputersViewModel.RefreshCollection();
            empAddWindow.Close();
        }, _ => Services.IsValidationProperties(Computer.ErrorCollection));
        
        public ICommand AddProccesorInCollectionCommand => new DelegateCommand(() => Computer.Processors_in_computers.Add(new Processors_in_computers()));

        public ICommand DeleteProccesorFromCollectionCommand => new DelegateCommand<Processors_in_computers>(procInComp => Computer.Processors_in_computers.Remove(procInComp));

        public ICommand AddOperatingSystemsInCollectionCommand => new DelegateCommand(() => Computer.Operating_systems_in_computers.Add(new Operating_systems_in_computers()));

        public ICommand DeleteOperatingSystemsFromCollectionCommand => new DelegateCommand<Operating_systems_in_computers>(operInComp => Computer.Operating_systems_in_computers.Remove(operInComp));

        public ICommand AddHddInCollectionCommand => new DelegateCommand(() => Computer.Hdd_in_computers.Add(new Hdd_in_computers()));

        public ICommand DeleteHddFromCollectionCommand => new DelegateCommand<Hdd_in_computers>(hddInComp => Computer.Hdd_in_computers.Remove(hddInComp));

        public ICommand AddRamInCollectionCommand => new DelegateCommand(() => Computer.Ram_in_computers.Add(new Ram_in_computers()));

        public ICommand DeleteRamFromCollectionCommand => new DelegateCommand<Ram_in_computers>(ramInComp => Computer.Ram_in_computers.Remove(ramInComp));

        public ICommand AddSsdInCollectionCommand => new DelegateCommand(() => Computer.Ssd_in_computers.Add(new Ssd_in_computers()));

        public ICommand DeleteSsdFromCollectionCommand => new DelegateCommand<Ssd_in_computers>(ramInComp => Computer.Ssd_in_computers.Remove(ramInComp));
        #endregion
    }
}
