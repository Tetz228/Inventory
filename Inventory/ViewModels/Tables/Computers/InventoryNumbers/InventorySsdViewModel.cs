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

    public class InventorySsdViewModel : BaseViewModel<Inventory_numbers_ssd>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации SSD-накопителей.xlsx";
        private const string NAMED_AREA_NAME = "InventorySsd";

        public InventorySsdViewModel() : base(InventorySsd, RefreshCollection, NAME_TEMPLATE, NAMED_AREA_NAME) => RefreshCollection();

        public static ObservableCollection<Inventory_numbers_ssd> InventorySsd { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            InventorySsd.Sort(inventorySsd => inventorySsd.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventorySsd.Sort(manufacturer => manufacturer.Ssd.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            InventorySsd.Sort(type => type.Ssd.Types_ssd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventorySsd.Sort(ssd => ssd.Ssd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            InventorySsd.Sort(ssd => ssd.Ssd.Memory_size + " " + ssd.Ssd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddInventorySsdCommand => new DelegateCommand(() =>
        {
            var addSsdWindow = new InventorySsdAddWindow();
            addSsdWindow.ShowDialog();
        });

        public ICommand EditInventorySsdCommand => new DelegateCommand<Inventory_numbers_ssd>(inventorySsd =>
        {
            var editWindow = new InventorySsdEditWindow();
            var editViewModel = new InventorySsdEditViewModel(inventorySsd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventorySsd => inventorySsd != null);

        public ICommand DeleteSsdCommand => new DelegateCommand<Inventory_numbers_ssd>(selectInventorySsd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарный SSD-накопитель:\nинвент. номер - {selectInventorySsd.Inventory_number};\nпроизводитель - {selectInventorySsd.Ssd.Manufacturer.Name};\nтип - {selectInventorySsd.Ssd.Types_ssd.Name};\nнаименование - {selectInventorySsd.Ssd.Name};\nОбъём - {selectInventorySsd.Ssd.Memory_size} {selectInventorySsd.Ssd.Unit.Short_name}?", "Удаление инвентарного SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_ssd>(selectInventorySsd.Id_inventory_number_ssd))
                InventorySsd.Remove(selectInventorySsd);
        }, selectInventorySsd => selectInventorySsd != null);

        public static void RefreshCollection()
        {
            InventorySsd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_ssd.AsNoTracking()
                .Include(manufacturer => manufacturer.Ssd.Manufacturer)
                .Include(unit => unit.Ssd.Unit)
                .Include(type => type.Ssd.Types_ssd))
            {
                InventorySsd.Add(item);
            }
            InventorySsd.Sort(ssd => ssd.Inventory_number);
        }
    }
}
