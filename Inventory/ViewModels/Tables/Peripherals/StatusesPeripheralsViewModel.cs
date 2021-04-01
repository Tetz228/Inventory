namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class StatusesPeripheralsViewModel : BindableBase
    {
        public StatusesPeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals);
            StatusesPeripherals.Sort(statusPeripheral => statusPeripheral.Name, SortDirection = ListSortDirection.Ascending);
            StatusesPeripheralsCollection = CollectionViewSource.GetDefaultView(StatusesPeripherals);
        }

        #region Свойства

        private ICollectionView StatusesPeripheralsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; set; }

        public Statuses_peripherals SelectStatusPeripheral { get; set; }

        private string _statusesPeripheralsFilter = string.Empty;

        public string StatusesPeripheralsFilter
        {
            get => _statusesPeripheralsFilter;
            set
            {
                _statusesPeripheralsFilter = value;
                StatusesPeripheralsCollection.Filter = obj =>
                {
                    if (obj is Statuses_peripherals statusPeripheral)
                        return statusPeripheral.Search(StatusesPeripheralsFilter);

                    return false;
                };
                StatusesPeripheralsCollection.Refresh();
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
                    case "Наименование":
                        {
                            StatusesPeripherals.Sort(statusPeripheral => statusPeripheral.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectStatusPeripheral = null;

        #endregion

        #region Команды
        public ICommand AddStatusPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new StatusPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditStatusPeripheralCommand => new DelegateCommand<Statuses_peripherals>(statusPeripheral =>
        {
            var editWindow = new StatusPeripheralEditWindow();
            var editViewModel = new StatusPeripheralEditViewModel(statusPeripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, statusPeripheral => statusPeripheral != null);

        public ICommand DeleteStatusPeripheralCommand => new DelegateCommand<Statuses_peripherals>(selectStatusPeripheral =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectStatusPeripheral.Name}?", "Удаление статус периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Statuses_peripherals>(selectStatusPeripheral.Id_status_peripheral);
            RefreshCollection();
        }, selectStatusPeripheral => selectStatusPeripheral != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            StatusesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_peripherals)
                StatusesPeripherals.Add(item);
        }
    }
}
