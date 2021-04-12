namespace Inventory.ViewModels.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class InventoryRamViewModel : BindableBase
    {
        public InventoryRamViewModel()
        {
            using var db = new InventoryEntities();

            InventoryRams = new ObservableCollection<Inventory_numbers_ram>(db.Inventory_numbers_ram.AsNoTracking().Include(manufacturer => manufacturer.Ram.Manufacturer).Include(unit => unit.Ram.Unit).Include(type => type.Ram.Types_memory));
            InventoryRams.Sort(manufacturer => manufacturer.Ram.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            InventoryRamsCollection = CollectionViewSource.GetDefaultView(InventoryRams);
        }

        #region Свойства
        private ICollectionView InventoryRamsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Inventory_numbers_ram SelectInventoryRam { get; set; }

        public static ObservableCollection<Inventory_numbers_ram> InventoryRams { get; set; }

        private string _inventoryRamsFilter = string.Empty;

        public string InventoryRamsFilter
        {
            get => _inventoryRamsFilter;
            set
            {
                _inventoryRamsFilter = value;
                InventoryRamsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_ram inventoryRam)
                        return inventoryRam.Search(InventoryRamsFilter);

                    return false;
                };
                InventoryRamsCollection.Refresh();
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
                            InventoryRams.Sort(inventoryRam => inventoryRam.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryRams.Sort(manufacturer => manufacturer.Ram.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryRams.Sort(ram => ram.Ram.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            InventoryRams.Sort(memory => memory.Ram.Memory_size, SortDirection);
                            break;
                        }
                    case "Единица измерения":
                        {
                            InventoryRams.Sort(unit => unit.Ram.Unit.Full_name, SortDirection);
                            break;
                        }
                    case "Тактовая частота":
                        {
                            InventoryRams.Sort(clock => clock.Ram.Clock_frequency, SortDirection);
                            break;
                        }
                    case "Тип памяти":
                        {
                            InventoryRams.Sort(type => type.Ram.Types_memory.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryRam = null;
        #endregion

        #region Команды
        public ICommand AddInventoryRamCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryRamAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryRamCommand => new DelegateCommand<Inventory_numbers_ram>(inventoryRam =>
        {
            var editWindow = new InventoryRamEditWindow();
            var editViewModel = new InventoryRamEditViewModel(inventoryRam);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, ram => ram != null);

        public ICommand DeleteInventoryRamCommand => new DelegateCommand<Inventory_numbers_ram>(selectInventoryRam =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryRam.Ram.Manufacturer.Name} {selectInventoryRam.Ram.Name} {selectInventoryRam.Ram.Memory_size} {selectInventoryRam.Ram.Unit.Short_name}?", "Удаление инвентарной оперативной памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_ram>(selectInventoryRam.Id_inventory_number_ram);
            RefreshCollection();
        }, selectRam => selectRam != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryRams.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_ram.AsNoTracking().Include(manufacturer => manufacturer.Ram.Manufacturer).Include(unit => unit.Ram.Unit).Include(type => type.Ram.Types_memory))
                InventoryRams.Add(item);
        }
    }
}
