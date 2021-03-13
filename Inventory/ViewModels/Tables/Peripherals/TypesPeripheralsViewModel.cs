namespace Inventory.ViewModels.Tables.Peripherals
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View.Add.Tables;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit;
    using Inventory.ViewModels.Edit.Tables.Peripherals;

    public class TypesPeripheralsViewModel : BindableBase
    {
        public TypesPeripheralsViewModel()
        {
            using var db = new InventoryEntities();
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals);
            TypesPeripheralsCollection = CollectionViewSource.GetDefaultView(TypesPeripherals);
            TypesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Types_peripherals> TypesPeripherals { get; set; }

        public ICollectionView TypesPeripheralsCollection { get; }

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
                        return Types_peripherals.Search(typePeripheral, TypesPeripheralsFilter);

                    return false;
                };
                TypesPeripheralsCollection.Refresh();
            }
        }
        #endregion

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                    {
                        if (TypesPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            TypesPeripheralsCollection.SortDescriptions.Clear();
                            TypesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            TypesPeripheralsCollection.SortDescriptions.Clear();
                            TypesPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Ascending));
                        }

                        TypesPeripheralsCollection.Refresh();

                        break;
                    }
            }
        }

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectTypePeripheral = null);

        public ICommand AddTypePeripheral => new DelegateCommand(() =>
        {
            var addTypePeripheralWindow = new TypePeripheralAddWindow();
            addTypePeripheralWindow.ShowDialog();
        });

        public ICommand EditTypePeripheral => new DelegateCommand<Types_peripherals>(typePeripheral =>
        {
            var editWindow = new TypePeripheralEditWindow();
            var editViewModel = new TypePeripheralEditViewModel(typePeripheral);

            editWindow.DataContext = editViewModel;
            editWindow.ShowDialog();

        }, typePeripheral => typePeripheral != null);

        public ICommand DeleteTypePeripheral => new DelegateCommand<Types_peripherals>(Types_peripherals.DeleteTypePeripheral, selectTypePeripheral => selectTypePeripheral != null);

        public ICommand RefreshList => new DelegateCommand(Types_peripherals.RefreshCollection);
        #endregion
    }
}
