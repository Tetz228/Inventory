namespace Inventory.ViewModels.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;

    public class InventoryNumbersHddViewModel : BindableBase
    {
        public InventoryNumbersHddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumbersHdds = new ObservableCollection<Inventory_numbers_hdd>(db.Inventory_numbers_hdd
                .Include(hdd => hdd.Hdd)
                .Include(manufacturer => manufacturer.Hdd.Manufacturer)
                .Include(type => type.Hdd.Types_hdd)
                .Include(unit => unit.Hdd.Unit));
            InventoryNumbersHdds.Sort(numberHdd => numberHdd.Inventory_number, SortDirection = ListSortDirection.Ascending);
            InventoryNumbersHddsCollection = CollectionViewSource.GetDefaultView(InventoryNumbersHdds);
        }

        #region Свойства

        private ICollectionView InventoryNumbersHddsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Inventory_numbers_hdd> InventoryNumbersHdds { get; set; }

        public Inventory_numbers_hdd SelectInventoryNumberHdd { get; set; }

        private string _inventoryNumbersHddsFilter = string.Empty;

        public string InventoryNumbersHddsFilter
        {
            get => _inventoryNumbersHddsFilter;
            set
            {
                _inventoryNumbersHddsFilter = value;
                InventoryNumbersHddsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_hdd inventoryNumberHdd)
                        return inventoryNumberHdd.Search(InventoryNumbersHddsFilter);

                    return false;
                };
                InventoryNumbersHddsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(numberHdd => numberHdd.Inventory_number, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(numberHdd => numberHdd.Inventory_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(manufacturer => manufacturer.Hdd.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(manufacturer => manufacturer.Hdd.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Тип":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(type => type.Hdd.Types_hdd.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(type => type.Hdd.Types_hdd.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(hdd => hdd.Hdd.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(hdd => hdd.Hdd.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Объем":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(memorySize => memorySize.Hdd.Memory_size, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(memorySize => memorySize.Hdd.Memory_size, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Единица измерения":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersHdds.Sort(unit => unit.Hdd.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersHdds.Sort(unit => unit.Hdd.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryNumberHdd = null;
        #endregion

        #region Команды
        public ICommand AddInventoryNumberHddCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryNumberHddAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryNumberHddCommand => new DelegateCommand<Inventory_numbers_hdd>(inventoryNumberHdd =>
        {
            var editWindow = new InventoryNumberHddEditWindow();
            var editViewModel = new InventoryNumbersHddEditViewModel(inventoryNumberHdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, hdd => hdd != null);

        public ICommand DeleteInventoryNumberHddCommand => new DelegateCommand<Inventory_numbers_hdd>(selectInventoryNumberHdd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryNumberHdd.Inventory_number}, {selectInventoryNumberHdd.Hdd.Manufacturer.Name} {selectInventoryNumberHdd.Hdd.Types_hdd.Name} {selectInventoryNumberHdd.Hdd.Name}?", "Удаление инвенторной периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_hdd>(selectInventoryNumberHdd.Id_inventory_number_hdd);
            RefreshCollection();
        }, selectInventoryNumberHdd => selectInventoryNumberHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryNumbersHdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_hdd
                .Include(hdd => hdd.Hdd)
                .Include(manufacturer => manufacturer.Hdd.Manufacturer)
                .Include(type => type.Hdd.Types_hdd)
                .Include(unit => unit.Hdd.Unit))
            {
                InventoryNumbersHdds.Add(item);
            }
        }
    }
}
