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

    public class StatusesPeripheralsViewModel : BaseViewModel<Statuses_peripherals>
    {
        public StatusesPeripheralsViewModel() : base(StatusesPeripherals, RefreshCollection) => RefreshCollection();

        public static ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить статус:\nнаименование - {selectStatusPeripheral.Name}?", "Удаление статус периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Statuses_peripherals>(selectStatusPeripheral.Id_status_peripheral))
                StatusesPeripherals.Remove(selectStatusPeripheral);
        }, selectStatusPeripheral => selectStatusPeripheral != null);

        private static void RefreshCollection()
        {
            StatusesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_peripherals.AsNoTracking())
            {
                StatusesPeripherals.Add(item);
            }
            StatusesPeripherals.Sort(statusPeripheral => statusPeripheral.Name);
        }
    }
}
