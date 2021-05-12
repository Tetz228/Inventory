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
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.Base;

    public class InventoryPowerSuppliesViewModel : BaseViewModel<Inventory_numbers_power_supplies>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации блоков питания.xlsx";
        private const string NAMED_AREA_NAME = "InventoryPowerSupplies";

        public InventoryPowerSuppliesViewModel() : base(InventoryPowerSupplies, RefreshCollection, NAME_TEMPLATE, NAMED_AREA_NAME) => RefreshCollection();

        public static ObservableCollection<Inventory_numbers_power_supplies> InventoryPowerSupplies { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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
                            InventoryPowerSupplies.Sort(powerSupply => powerSupply.Power_supplies.Power + " " + powerSupply.Power_supplies.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарный блок питания:\nинвент. номер - {selectInventoryPowerSupplies.Inventory_number};\nпроизводитель - {selectInventoryPowerSupplies.Power_supplies.Manufacturer.Name};\nнаименование - {selectInventoryPowerSupplies.Power_supplies.Name};\nмощность -{selectInventoryPowerSupplies.Power_supplies.Power} {selectInventoryPowerSupplies.Power_supplies.Unit.Short_name}?", "Удаление инвентарного блока питания", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_power_supplies>(selectInventoryPowerSupplies.Id_inventory_number_power_supplie))
                InventoryPowerSupplies.Remove(selectInventoryPowerSupplies);
        }, selectInventoryPowerSupply => selectInventoryPowerSupply != null);

        public static void RefreshCollection()
        {
            InventoryPowerSupplies.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_power_supplies.AsNoTracking()
                .Include(manufacturer => manufacturer.Power_supplies.Manufacturer)
                .Include(unit => unit.Power_supplies.Unit))
            {
                InventoryPowerSupplies.Add(item);
            }
            InventoryPowerSupplies.Sort(inventoryPowerSupplies => inventoryPowerSupplies.Inventory_number);
        }
    }
}
