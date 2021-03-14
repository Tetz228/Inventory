namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class PeripheralsViewModel : BindableBase
    {
        public PeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.Include(manufacturer => manufacturer.Manufacturer).Include(typePeripheral => typePeripheral.Types_peripherals));
            PeripheralsCollection = CollectionViewSource.GetDefaultView(Peripherals);
            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Peripheral.Types_peripherals.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Peripheral> Peripherals { get; set; }

        public ICollectionView PeripheralsCollection { get; }

        public Peripheral SelectPeripheral { get; set; }

        private string _peripheralsFilter = string.Empty;

        public string PeripheralsFilter
        {
            get => _peripheralsFilter;
            set
            {
                _peripheralsFilter = value;
                PeripheralsCollection.Filter = obj =>
                {
                    if (obj is Peripheral peripheral)
                        return Peripheral.Search(peripheral, PeripheralsFilter);

                    return false;
                };
                PeripheralsCollection.Refresh();
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
                case "Тип периферии":
                    {
                        if (PeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Ascending));
                        }
                        PeripheralsCollection.Refresh();
                        break;
                    }
                case "Наименование":
                    {
                        if (PeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Peripheral.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Peripheral.Name), ListSortDirection.Ascending));
                        }
                        PeripheralsCollection.Refresh();
                        break;
                    }
                case "Производитель":
                    {
                        if (PeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Manufacturer.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            PeripheralsCollection.SortDescriptions.Clear();
                            PeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Manufacturer.Name), ListSortDirection.Ascending));
                        }
                        PeripheralsCollection.Refresh();
                        break;
                    }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddPeripheral => new DelegateCommand(() =>
        {
            var addWindow = new PeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditPeripheral => new DelegateCommand<Peripheral>(peripheral =>
        {
            var editWindow = new PeripheralEditWindow();
            var editViewModel = new PeripheralEditViewModel(peripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, peripheral => peripheral != null);

        public ICommand DeletePeripheral => new DelegateCommand<Peripheral>(Peripheral.DeletePeripheral, selectPeripheral => selectPeripheral != null);

        public ICommand RefreshList => new DelegateCommand(Peripheral.RefreshCollection);
        #endregion
    }
}