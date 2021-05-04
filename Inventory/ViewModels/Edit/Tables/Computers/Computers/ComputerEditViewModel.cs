namespace Inventory.ViewModels.Edit.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class ComputerEditViewModel : BindableBase
    {
        private delegate void EditCollections(int id);

        public ComputerEditViewModel(Computer computer)
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
            InventoryHdds = new ObservableCollection<Inventory_numbers_hdd>(db.Inventory_numbers_hdd
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

            Computer = computer;
            Computer.BeginEdit();

            _editCollections = EditProccesorsInComputer;
            _editCollections += EditOperatingSystemsInComputer;
            _editCollections += EditHddsInComputer;
            _editCollections += EditRamsInComputer;
            _editCollections += EditSsdsInComputer;
        }

        private readonly EditCollections _editCollections;

        #region Свойства

        public Computer Computer { get; }

        public ObservableCollection<Inventory_numbers_motherboards> InventoryMotherboards { get; }

        public ObservableCollection<Inventory_numbers_graphics_cards> InventoryGraphicsCards { get; }

        public ObservableCollection<Inventory_numbers_power_supplies> InventoryPowerSupplies { get; }

        public ObservableCollection<Inventory_numbers_processors> InventoryProcessors { get; }

        public ObservableCollection<Inventory_numbers_ssd> InventorySsds { get; }

        public ObservableCollection<Inventory_numbers_ram> InventoryRams { get; }

        public ObservableCollection<Statuses_computers> StatusesComputers { get; }

        public ObservableCollection<Operating_systems> OperatingSystems { get; }

        public ObservableCollection<Inventory_numbers_hdd> InventoryHdds { get; }

        #endregion

        public void OnWindowClosing(object sender, CancelEventArgs e) => Computer.CancelEdit();

        #region Команды
        public ICommand EditComputerCommand => new DelegateCommand<Window>(addWindow =>
        {
            Computer.EndEdit();
            Services.Edit(Computer.Id_computer, Computer);
            _editCollections(Computer.Id_computer);

            ComputersViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Computer.ErrorCollection));

        public ICommand AddProccesorInCollectionCommand => new DelegateCommand(() => Computer.Processors_in_computers.Add(new Processors_in_computers()));

        public ICommand DeleteProccesorFromCollectionCommand => new DelegateCommand<Processors_in_computers>(selectProcInComp =>
        {
            if (selectProcInComp.Id_processor_in_computer != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить процессор \"{selectProcInComp.Inventory_numbers_processors.Processor.Name}\" у компьютера? Это действие нельзя отменить.",
                    "Удаление заданного процессора", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Processors_in_computers>(selectProcInComp.Id_processor_in_computer);
            }
            Computer.Processors_in_computers.Remove(selectProcInComp);
        });

        public ICommand AddOperatingSystemsInCollectionCommand => new DelegateCommand(() => Computer.Operating_systems_in_computers.Add(new Operating_systems_in_computers()));

        public ICommand DeleteOperatingSystemsFromCollectionCommand => new DelegateCommand<Operating_systems_in_computers>(selectOperInComp =>
        {
            if (selectOperInComp.Id_operating_systems_in_computers != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить операционную систему \"{selectOperInComp.Operating_systems.Name}\" у компьютера? Это действие нельзя отменить.",
                    "Удаление заданной операционной системы", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Operating_systems_in_computers>(selectOperInComp.Id_operating_systems_in_computers);
            }
            Computer.Operating_systems_in_computers.Remove(selectOperInComp);
        });

        public ICommand AddHddInCollectionCommand => new DelegateCommand(() => Computer.Hdd_in_computers.Add(new Hdd_in_computers()));

        public ICommand DeleteHddFromCollectionCommand => new DelegateCommand<Hdd_in_computers>(selectHddInComp =>
        {
            if(selectHddInComp.Id_hdd_in_computer != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить жесткий диск \"{selectHddInComp.Inventory_numbers_hdd.Hdd.Name}\" у компьютера? Это действие нельзя отменить.",
                    "Удаление заданного жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Hdd_in_computers>(selectHddInComp.Id_hdd_in_computer);
            }
            Computer.Hdd_in_computers.Remove(selectHddInComp);
        });

        public ICommand AddRamInCollectionCommand => new DelegateCommand(() => Computer.Ram_in_computers.Add(new Ram_in_computers()));

        public ICommand DeleteRamFromCollectionCommand => new DelegateCommand<Ram_in_computers>(selectRamInComp =>
        {
            if (selectRamInComp.Id_ram_in_computer != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить оперативную память \"{selectRamInComp.Inventory_numbers_ram.Ram.Name}\" у компьютера? Это действие нельзя отменить.",
                    "Удаление заданной оперативной памяти", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Ram_in_computers>(selectRamInComp.Id_ram_in_computer);
            }
            Computer.Ram_in_computers.Remove(selectRamInComp);
        });

        public ICommand AddSsdInCollectionCommand => new DelegateCommand(() => Computer.Ssd_in_computers.Add(new Ssd_in_computers()));

        public ICommand DeleteSsdFromCollectionCommand => new DelegateCommand<Ssd_in_computers>(selectSsdInComp =>
        {
            if (selectSsdInComp.Id_ssd_in_computer != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить SSD-накопитель \"{selectSsdInComp.Inventory_numbers_ssd.Ssd.Name}\" у компьютера? Это действие нельзя отменить.",
                    "Удаление заданного SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Ssd_in_computers>(selectSsdInComp.Id_ssd_in_computer);
            }
            Computer.Ssd_in_computers.Remove(selectSsdInComp);
        });
        #endregion

        #region Методы редактирования коллекций

        private void EditProccesorsInComputer(int idComputer)
        {
            using var db = new InventoryEntities();

            foreach (var item in Computer.Processors_in_computers)
            {
                if (item.Id_processor_in_computer == 0)
                {
                    item.Fk_computer = idComputer;
                    db.Processors_in_computers.Add(item);
                }
                else
                {
                    var processorInComputer = db.Processors_in_computers.FirstOrDefault(empDepart => empDepart.Id_processor_in_computer == item.Id_processor_in_computer);
                    if (processorInComputer != null)
                        processorInComputer.Fk_inventory_number_processor = item.Fk_inventory_number_processor;
                }
            }
            db.SaveChanges();
        }

        private void EditOperatingSystemsInComputer(int idComputer)
        {
            using var db = new InventoryEntities();

            foreach (var item in Computer.Operating_systems_in_computers)
            {
                if (item.Id_operating_systems_in_computers == 0)
                {
                    item.Fk_computer = idComputer;
                    db.Operating_systems_in_computers.Add(item);
                }
                else
                {
                    var systemInComputer = db.Operating_systems_in_computers.FirstOrDefault(operatingSystemsInComputers => operatingSystemsInComputers.Id_operating_systems_in_computers == item.Id_operating_systems_in_computers);
                    if (systemInComputer != null)
                        systemInComputer.Fk_operating_system = item.Fk_operating_system;
                }
            }
            db.SaveChanges();
        }

        private void EditHddsInComputer(int idComputer)
        {
            using var db = new InventoryEntities();

            foreach (var item in Computer.Hdd_in_computers)
            {
                if (item.Id_hdd_in_computer == 0)
                {
                    item.Fk_computer = idComputer;
                    db.Hdd_in_computers.Add(item);
                }
                else
                {
                    var hddInComputer = db.Hdd_in_computers.FirstOrDefault(hddInComputers => hddInComputers.Id_hdd_in_computer == item.Id_hdd_in_computer);
                    if (hddInComputer != null)
                        hddInComputer.Fk_inventory_number_hdd = item.Fk_inventory_number_hdd;
                }
            }
            db.SaveChanges();
        }

        private void EditRamsInComputer(int idComputer)
        {
            using var db = new InventoryEntities();

            foreach (var item in Computer.Ram_in_computers)
            {
                if (item.Id_ram_in_computer == 0)
                {
                    item.Fk_computer = idComputer;
                    db.Ram_in_computers.Add(item);
                }
                else
                {
                    var ramInComputer = db.Ram_in_computers.FirstOrDefault(ramInComputers => ramInComputers.Id_ram_in_computer == item.Id_ram_in_computer);
                    if (ramInComputer != null)
                        ramInComputer.Fk_inventory_number_ram = item.Fk_inventory_number_ram;
                }
            }
            db.SaveChanges();
        }

        private void EditSsdsInComputer(int idComputer)
        {
            using var db = new InventoryEntities();

            foreach (var item in Computer.Ssd_in_computers)
            {
                if (item.Id_ssd_in_computer == 0)
                {
                    item.Fk_computer = idComputer;
                    db.Ssd_in_computers.Add(item);
                }
                else
                {
                    var ssdInComputer = db.Ssd_in_computers.FirstOrDefault(ssdInComputers => ssdInComputers.Id_ssd_in_computer == item.Id_ssd_in_computer);
                    if (ssdInComputer != null)
                        ssdInComputer.Fk_inventory_number_ssd = item.Fk_inventory_number_ssd;
                }
            }
            db.SaveChanges();
        }

        #endregion
    }
}