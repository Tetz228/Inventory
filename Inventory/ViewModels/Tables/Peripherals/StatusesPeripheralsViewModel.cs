namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    class StatusesPeripheralsViewModel : BindableBase
    {
        public StatusesPeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals);
            StatusesPeripheralsCollection = CollectionViewSource.GetDefaultView(StatusesPeripherals);
            StatusesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Statuses_peripherals.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; set; }

        public ICollectionView StatusesPeripheralsCollection { get; }

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
                        return Statuses_peripherals.Search(statusPeripheral, StatusesPeripheralsFilter);

                    return false;
                };
                StatusesPeripheralsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                    {
                        if (StatusesPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            StatusesPeripheralsCollection.SortDescriptions.Clear();
                            StatusesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Statuses_peripherals.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            StatusesPeripheralsCollection.SortDescriptions.Clear();
                            StatusesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Statuses_peripherals.Name), ListSortDirection.Ascending));
                        }
                        StatusesPeripheralsCollection.Refresh();
                        break;
                    }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectStatusPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddStatusPeripheral => new DelegateCommand(() =>
        {
            var addWindow = new StatusPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditStatusPeripheral => new DelegateCommand<Statuses_peripherals>(statusPeripheral =>
        {
            var editWindow = new StatusPeripheralEditWindow();
            var editViewModel = new StatusPeripheralEditViewModel(statusPeripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, statusPeripheral => statusPeripheral != null);

        public ICommand DeleteStatusPeripheral => new DelegateCommand<Statuses_peripherals>(Statuses_peripherals.DeleteStatusPeripheral, selectStatusPeripheral => selectStatusPeripheral != null);

        public ICommand RefreshList => new DelegateCommand(Statuses_peripherals.RefreshCollection);
        #endregion
    }
}
