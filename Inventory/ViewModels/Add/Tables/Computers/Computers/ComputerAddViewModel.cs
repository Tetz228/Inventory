namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using Inventory.ViewModels.Tables.Employees;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class ComputerAddViewModel : BindableBase
    {
        public ComputerAddViewModel()
        {
            using var db = new InventoryEntities();

            FillingCollections(db);

            try
            {
                Computer.Inventory_number = db.Computers.Select(computer => computer.Inventory_number).Max() + 1;
            }
            catch
            {
                Computer.Inventory_number = 1;
            }
        }

        private void FillingCollections(InventoryEntities db)
        {
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

            StatusesComputers = new ObservableCollection<Statuses_computers>(db.Statuses_computers).Sort(status => status.Name);
            OperatingSystems = new ObservableCollection<Operating_systems>(db.Operating_systems).Sort(operatingSystems => operatingSystems.Name);
        }

        #region Свойства

        public Computer Computer { get; } = new();

        public ObservableCollection<Inventory_numbers_motherboards> InventoryMotherboards { get; set; }

        public ObservableCollection<Inventory_numbers_graphics_cards> InventoryGraphicsCards { get; set; }

        public ObservableCollection<Inventory_numbers_power_supplies> InventoryPowerSupplies { get; set; }

        public ObservableCollection<Inventory_numbers_processors> InventoryProcessors { get; set; }

        public ObservableCollection<Statuses_computers> StatusesComputers { get; set; }

        public ObservableCollection<Operating_systems> OperatingSystems { get; set; }

        #endregion

        #region Команды
        public ICommand AddComputerCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Services.Add(Computer);
            ComputersViewModel.RefreshCollection();
            empAddWindow.Close();
        }, _ => Services.IsValidationProperties(Computer.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddProccesorInCollectionCommand => new DelegateCommand(() =>
        {
            Computer.Processors_in_computers.Add(new Processors_in_computers());
            RaisePropertyChanged(nameof(Computer.Processors_in_computers));
        });

        public ICommand DeleteProccesorFromCollectionCommand => new DelegateCommand<Processors_in_computers>(procInComp => Computer.Processors_in_computers.Remove(procInComp));

        public ICommand AddOperatingSystemsInCollectionCommand => new DelegateCommand(() => Computer.Operating_systems_in_computers.Add(new Operating_systems_in_computers()));

        public ICommand DeleteOperatingSystemsFromCollectionCommand => new DelegateCommand<Operating_systems_in_computers>(operInComp => Computer.Operating_systems_in_computers.Remove(operInComp));

        #endregion
    }
}
