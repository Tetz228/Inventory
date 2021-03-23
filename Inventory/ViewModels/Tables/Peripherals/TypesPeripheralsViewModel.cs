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

    public class TypesPeripheralsViewModel : BindableBase
    {
        public TypesPeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals);
            TypesPeripherals.Sort(typePeripheral => typePeripheral.Name, SortDirection = ListSortDirection.Ascending);
            TypesPeripheralsCollection = CollectionViewSource.GetDefaultView(TypesPeripherals);
        }

        #region Свойства

        private ICollectionView TypesPeripheralsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Types_peripherals> TypesPeripherals { get; set; }

        public Types_peripherals SelectTypePeripheral { get; set; }

        private string _typesPeripheralsFilter = string.Empty;

        public string TypesPeripheralsFilter
        {
            get => _typesPeripheralsFilter;
            set
            {
                _typesPeripheralsFilter = value;
                TypesPeripheralsCollection.Filter = obj =>
                {
                    if (obj is Types_peripherals typePeripheral)
                        return Types_peripherals.SearchFor(typePeripheral, TypesPeripheralsFilter);

                    return false;
                };
                TypesPeripheralsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            TypesPeripherals.Sort(typePeripheral => typePeripheral.Name, SortDirection = ListSortDirection.Descending);
                        else
                            TypesPeripherals.Sort(typePeripheral => typePeripheral.Name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
            }
        }
        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectTypePeripheral = null;
        #endregion

        #region Команды
        public ICommand AddTypePeripheralCommand => new DelegateCommand(() =>
        {
            var addTypePeripheralWindow = new TypePeripheralAddWindow();
            addTypePeripheralWindow.ShowDialog();
        });

        public ICommand EditTypePeripheralCommand => new DelegateCommand<Types_peripherals>(typePeripheral =>
        {
            var editWindow = new TypePeripheralEditWindow();
            var editViewModel = new TypePeripheralEditViewModel(typePeripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();

        }, typePeripheral => typePeripheral != null);

        public ICommand DeleteTypePeripheralCommand => new DelegateCommand<Types_peripherals>(Types_peripherals.DeleteTypePeripheral, selectTypePeripheral => selectTypePeripheral != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Types_peripherals.RefreshCollection);
        #endregion
    }
}
