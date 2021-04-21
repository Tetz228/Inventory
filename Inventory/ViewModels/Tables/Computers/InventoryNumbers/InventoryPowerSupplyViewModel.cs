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

    public class InventoryPowerSupplyViewModel : BindableBase
    {
        public InventoryPowerSupplyViewModel()
        {
            using var db = new InventoryEntities();

            InventoryPowerSupplies = new ObservableCollection<Inventory_numbers_power_supplies>(db.Inventory_numbers_power_supplies.AsNoTracking().Include(manufacturer => manufacturer.Power_supplies.Manufacturer).Include(unit => unit.Power_supplies.Unit));
            InventoryPowerSupplies.Sort(inventoryPowerSupplies => inventoryPowerSupplies.Inventory_number, SortDirection = ListSortDirection.Ascending);
            InventoryPowerSuppliesCollection = CollectionViewSource.GetDefaultView(InventoryPowerSupplies);
        }

        #region Свойства
        private ICollectionView InventoryPowerSuppliesCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Inventory_numbers_power_supplies SelectInventoryPowerSupply { get; set; }

        public static ObservableCollection<Inventory_numbers_power_supplies> InventoryPowerSupplies { get; set; }

        private string _inventoryPowerSuppliesFilter = string.Empty;

        public string InventoryPowerSuppliesFilter
        {
            get => _inventoryPowerSuppliesFilter;
            set
            {
                _inventoryPowerSuppliesFilter = value;
                InventoryPowerSuppliesCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_power_supplies inventoryPowerSupply)
                        return inventoryPowerSupply.Search(InventoryPowerSuppliesFilter);

                    return false;
                };
                InventoryPowerSuppliesCollection.Refresh();
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
                            InventoryPowerSupplies.Sort(inventoryPowerSupplies => inventoryPowerSupplies.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryPowerSupplies.Sort(manufacturer => manufacturer.Power_supplies.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryPowerSupplies.Sort(powerSupply => powerSupply.Power_supplies.Name, SortDirection);
                            break;
                        }
                    case "Мощность":
                        {
                            InventoryPowerSupplies.Sort(powerSupply => powerSupply.Power_supplies.Power, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryPowerSupply = null;
        #endregion

        #region Команды
        public ICommand AddInventoryPowerSupplyCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryPowerSupplyAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryPowerSupplyCommand => new DelegateCommand<Inventory_numbers_power_supplies>(inventoryPowerSupplies =>
        {
            var editWindow = new InventoryPowerSupplyEditWindow();
            var editViewModel = new InventoryPowerSupplyEditViewModel(inventoryPowerSupplies);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryPowerSupplies => inventoryPowerSupplies != null);

        public ICommand DeleteInventoryPowerSupplyCommand => new DelegateCommand<Inventory_numbers_power_supplies>(selectInventoryPowerSupplies =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryPowerSupplies.Inventory_number} {selectInventoryPowerSupplies.Power_supplies.Manufacturer.Name} {selectInventoryPowerSupplies.Power_supplies.Name} {selectInventoryPowerSupplies.Power_supplies.Power} {selectInventoryPowerSupplies.Power_supplies.Unit.Short_name}?", "Удаление инвентарного блока питания", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_power_supplies>(selectInventoryPowerSupplies.Id_inventory_number_power_supplie);
            RefreshCollection();
        }, selectInventoryPowerSupply => selectInventoryPowerSupply != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryPowerSupplies.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_power_supplies.AsNoTracking().Include(manufacturer => manufacturer.Power_supplies.Manufacturer).Include(unit => unit.Power_supplies.Unit))
                InventoryPowerSupplies.Add(item);
        }
    }
}
