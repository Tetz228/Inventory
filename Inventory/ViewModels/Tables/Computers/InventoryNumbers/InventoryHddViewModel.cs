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

    public class InventoryHddViewModel : BindableBase
    {
        private const string NAME_TEMPLATE = "TemplateInventoryHdd.xlsx";
        private const string NAMED_AREA_NAME = "InventoryHdd";

        public InventoryHddViewModel()
        {
            RefreshCollection();
            InventoryHddCollection = CollectionViewSource.GetDefaultView(InventoryHdd);
        }

        #region Свойства

        public ICollectionView InventoryHddCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Inventory_numbers_hdd> InventoryHdd { get; set; } = new();

        public Inventory_numbers_hdd SelectInventoryHdd { get; set; }

        private string _inventoryHddFilter = string.Empty;

        public string InventoryHddFilter
        {
            get => _inventoryHddFilter;
            set
            {
                _inventoryHddFilter = value;
                InventoryHddCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_hdd inventoryHdd)
                        return inventoryHdd.Search(InventoryHddFilter);

                    return false;
                };
                InventoryHddCollection.Refresh();
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
                            InventoryHdd.Sort(numberHdd => numberHdd.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryHdd.Sort(manufacturer => manufacturer.Hdd.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            InventoryHdd.Sort(type => type.Hdd.Types_hdd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryHdd.Sort(hdd => hdd.Hdd.Name, SortDirection);
                            break;
                        }
                    case "Объем":
                        {
                            InventoryHdd.Sort(memorySize => memorySize.Hdd.Memory_size + " " + memorySize.Hdd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryHdd = null;
        #endregion

        #region Команды
        public ICommand AddInventoryHddCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryHddAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryHddCommand => new DelegateCommand<Inventory_numbers_hdd>(inventoryHdd =>
        {
            var editWindow = new InventoryHddEditWindow();
            var editViewModel = new InventoryHddEditViewModel(inventoryHdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryHdd => inventoryHdd != null);

        public ICommand DeleteInventoryHddCommand => new DelegateCommand<Inventory_numbers_hdd>(selectInventoryHdd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарный жесткий диск:\nинвент. номер - {selectInventoryHdd.Inventory_number};\nпроизводитель - {selectInventoryHdd.Hdd.Manufacturer.Name};\nтип - {selectInventoryHdd.Hdd.Types_hdd.Name};\nнаименование - {selectInventoryHdd.Hdd.Name};\nобъём - {selectInventoryHdd.Hdd.Memory_size} {selectInventoryHdd.Hdd.Unit.Short_name}?", "Удаление инвентарного жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_hdd>(selectInventoryHdd.Id_inventory_number_hdd);
            InventoryHdd.Remove(selectInventoryHdd);
        }, selectInventoryHdd => selectInventoryHdd != null);

        public ICommand ExportExcelCommand => new DelegateCommand<ICollectionView>(collectionView => collectionView.ExportExcel(NAME_TEMPLATE, NAMED_AREA_NAME));

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryHdd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_hdd.AsNoTracking()
                .Include(hdd => hdd.Hdd)
                .Include(manufacturer => manufacturer.Hdd.Manufacturer)
                .Include(type => type.Hdd.Types_hdd)
                .Include(unit => unit.Hdd.Unit))
            {
                InventoryHdd.Add(item);
            }

            InventoryHdd.Sort(numberHdd => numberHdd.Inventory_number);
        }
    }
}
