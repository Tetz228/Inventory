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

    using Inventory.Services;

    public class PeripheralsViewModel : BindableBase
    {
        public PeripheralsViewModel()
        {
            RefreshCollection();
            PeripheralsCollection = CollectionViewSource.GetDefaultView(Peripherals);
        }

        #region Свойства
        private ICollectionView PeripheralsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Peripheral> Peripherals { get; set; } = new();

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
                        return peripheral.Search(PeripheralsFilter);

                    return false;
                };
                PeripheralsCollection.Refresh();
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
                    case "Производитель":
                        {
                            Peripherals.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип периферии":
                        {
                            Peripherals.Sort(type => type.Types_peripherals.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Peripherals.Sort(peripheral => peripheral.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new PeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditPeripheralCommand => new DelegateCommand<Peripheral>(peripheral =>
        {
            var editWindow = new PeripheralEditWindow();
            var editViewModel = new PeripheralEditViewModel(peripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, peripheral => peripheral != null);

        public ICommand DeletePeripheralCommand => new DelegateCommand<Peripheral>(selectPeripheral =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить периферию:\nпроизводитель - {selectPeripheral.Manufacturer.Name};\nтип - {selectPeripheral.Types_peripherals.Name};\nнаименование - {selectPeripheral.Name}?", "Удаление периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Peripheral>(selectPeripheral.Id_peripheral);
            Peripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Peripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Peripherals.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(typePeripheral => typePeripheral.Types_peripherals))
            {
                Peripherals.Add(item);
            }
            Peripherals.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}