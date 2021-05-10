namespace Inventory.ViewModels.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Computers;
    using Inventory.View.Edit.Tables.Computers.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.ViewModels.Edit.Tables.Computers.Computers;

    public class ComputersViewModel : BindableBase
    {
        public ComputersViewModel()
        {
            RefreshCollection();
            ComputersCollection = CollectionViewSource.GetDefaultView(Computers);
        }

        #region Свойства
        private ICollectionView ComputersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Computer SelectComputer { get; set; }

        public static ObservableCollection<Computer> Computers { get; set; } = new();

        private string _computersFilter = string.Empty;

        public string ComputersFilter
        {
            get => _computersFilter;
            set
            {
                _computersFilter = value;
                ComputersCollection.Filter = obj =>
                {
                    if (obj is Computer computer)
                        return computer.Search(ComputersFilter);

                    return false;
                };
                ComputersCollection.Refresh();
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
                    case "Инвентарный номер":
                        {
                            Computers.Sort(inventoryNumber => inventoryNumber.Inventory_number, SortDirection);
                            break;
                        }
                    case "IP-адрес":
                        {
                            Computers.Sort(computer => computer.Ip_address, SortDirection);
                            break;
                        }
                    case "Статус":
                        {
                            Computers.Sort(status => status.Statuses_computers.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectComputer = null;
        #endregion

        #region Команды
        public ICommand AddComputerCommand => new DelegateCommand(() =>
        {
            var addWindow = new ComputerAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditComputerCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            var editWindow = new ComputerEditWindow();
            var editViewModel = new ComputerEditViewModel(selectComputer);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, computer => computer != null);

        public ICommand DeleteComputerCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить компьютер:\nинверт. номер - {selectComputer.Inventory_number};\nIP-адрес - {selectComputer.Ip_address};\nСтатус - {selectComputer.Statuses_computers.Name}?", "Удаление компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Computer>(selectComputer.Id_computer);
            Computers.Remove(selectComputer);
        }, selectComputer => selectComputer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Computers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Computers.AsNoTracking()
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
            {
                Computers.Add(item);
            }

            Computers.Sort(computer => computer.Inventory_number);
        }
    }
}
