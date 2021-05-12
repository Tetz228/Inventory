namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TypesPeripheralsViewModel : BaseViewModel<Types_peripherals>
    {
        public TypesPeripheralsViewModel() : base(TypesPeripherals, RefreshCollection) => RefreshCollection();

        public static ObservableCollection<Types_peripherals> TypesPeripherals { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            TypesPeripherals.Sort(typePeripheral => typePeripheral.Name, SortDirection);
                            break;
                        }
                }
            }
        }

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

        public ICommand DeleteTypePeripheralCommand => new DelegateCommand<Types_peripherals>(selectTypePeripheral =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить тип:\nнаименование -  {selectTypePeripheral.Name}?", "Удаление типа периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Types_peripherals>(selectTypePeripheral.Id_type_peripheral))
                TypesPeripherals.Remove(selectTypePeripheral);
        }, selectTypePeripheral => selectTypePeripheral != null);

        private static void RefreshCollection()
        {
            TypesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_peripherals.AsNoTracking())
            {
                TypesPeripherals.Add(item);
            }
            TypesPeripherals.Sort(typePeripheral => typePeripheral.Name);
        }
    }
}
