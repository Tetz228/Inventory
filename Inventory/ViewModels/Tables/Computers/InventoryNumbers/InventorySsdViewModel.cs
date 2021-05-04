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

    public class InventorySsdViewModel : BindableBase
    {
        public InventorySsdViewModel()
        {
            RefreshCollection();
            InventorySsdsCollection = CollectionViewSource.GetDefaultView(InventorySsds);
        }

        #region Свойства
        private ICollectionView InventorySsdsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Inventory_numbers_ssd SelectInventorySsd { get; set; }

        public static ObservableCollection<Inventory_numbers_ssd> InventorySsds { get; set; } = new();

        private string _ssdsFilter = string.Empty;

        public string InventorySsdsFilter
        {
            get => _ssdsFilter;
            set
            {
                _ssdsFilter = value;
                InventorySsdsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_ssd ssd)
                        return ssd.Search(InventorySsdsFilter);

                    return false;
                };
                InventorySsdsCollection.Refresh();
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
                            InventorySsds.Sort(inventorySsd => inventorySsd.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventorySsds.Sort(manufacturer => manufacturer.Ssd.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            InventorySsds.Sort(type => type.Ssd.Types_ssd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventorySsds.Sort(ssd => ssd.Ssd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            InventorySsds.Sort(ssd => ssd.Ssd.Memory_size + " " + ssd.Ssd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventorySsd = null;
        #endregion

        #region Команды
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

            Services.Delete<Inventory_numbers_ssd>(selectInventorySsd.Id_inventory_number_ssd);
            InventorySsds.Remove(selectInventorySsd);
        }, selectInventorySsd => selectInventorySsd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventorySsds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_ssd.AsNoTracking()
                .Include(manufacturer => manufacturer.Ssd.Manufacturer)
                .Include(unit => unit.Ssd.Unit)
                .Include(type => type.Ssd.Types_ssd))
            {
                InventorySsds.Add(item);
            }
            InventorySsds.Sort(ssd => ssd.Inventory_number);
        }
    }
}
