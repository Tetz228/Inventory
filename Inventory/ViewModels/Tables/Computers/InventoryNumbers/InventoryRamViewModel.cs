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

    public class InventoryRamViewModel : BaseViewModel<Inventory_numbers_ram>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации оперативной памяти.xlsx";
        private const string NAMED_AREA_NAME = "InventoryRam";

        public InventoryRamViewModel() : base(InventoryRams) => RefreshCollection();

        public static ObservableCollection<Inventory_numbers_ram> InventoryRams { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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
                            InventoryRams.Sort(ram => ram.Ram.Name + " " + ram.Ram.Unit.Short_name, SortDirection);
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
                }
            }
        }

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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить оператив. память:\nинвент. номер - {selectInventoryRam.Inventory_number};\nпроизводитель - {selectInventoryRam.Ram.Manufacturer.Name};\nнаименование - {selectInventoryRam.Ram.Name};\nтип памяти - {selectInventoryRam.Ram.Types_memory.Name};\nобъём - {selectInventoryRam.Ram.Memory_size} {selectInventoryRam.Ram.Unit.Short_name};\nтактовая частота - {selectInventoryRam.Ram.Clock_frequency}?", "Удаление инвентарной оперативной памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_ram>(selectInventoryRam.Id_inventory_number_ram))
                InventoryRams.Remove(selectInventoryRam);
        }, selectRam => selectRam != null);

        public ICommand ExportExcelCommand => new DelegateCommand<ICollectionView>(collectionView => collectionView.ExportExcel(NAME_TEMPLATE, NAMED_AREA_NAME));

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        
        public static void RefreshCollection()
        {
            InventoryRams.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_ram.AsNoTracking()
                .Include(manufacturer => manufacturer.Ram.Manufacturer)
                .Include(unit => unit.Ram.Unit)
                .Include(type => type.Ram.Types_memory))
            {
                InventoryRams.Add(item);
            }
            InventoryRams.Sort(ram => ram.Inventory_number);
        }
    }
}
