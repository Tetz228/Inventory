namespace Inventory.ViewModels.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class InventoryProcessorViewModel : BindableBase
    {
        public InventoryProcessorViewModel()
        {
            RefreshCollection();
            InventoryProcessorsCollection = CollectionViewSource.GetDefaultView(InventoryProcessors);
        }

        #region Свойства
        private ICollectionView InventoryProcessorsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Inventory_numbers_processors SelectInventoryProcessor { get; set; }

        public static ObservableCollection<Inventory_numbers_processors> InventoryProcessors { get; set; } = new();

        private string _inventoryProcessorsFilter = string.Empty;

        public string InventoryProcessorsFilter
        {
            get => _inventoryProcessorsFilter;
            set
            {
                _inventoryProcessorsFilter = value;
                InventoryProcessorsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_processors inventoryProcessor)
                        return inventoryProcessor.Search(InventoryProcessorsFilter);

                    return false;
                };
                InventoryProcessorsCollection.Refresh();
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
                            InventoryProcessors.Sort(inventoryProcessor => inventoryProcessor.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryProcessors.Sort(manufacturer => manufacturer.Processor.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryProcessors.Sort(processor => processor.Processor.Name, SortDirection);
                            break;
                        }
                    case "Сокет":
                        {
                            InventoryProcessors.Sort(processor => processor.Processor.Socket.Name, SortDirection);
                            break;
                        }
                    case "Количество ядер":
                        {
                            InventoryProcessors.Sort(processor => processor.Processor.Amount_cores, SortDirection);
                            break;
                        }
                    case "Базовая частота":
                        {
                            InventoryProcessors.Sort(processor => processor.Processor.Base_frequency + " " + processor.Processor.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryProcessor = null;
        #endregion

        #region Команды
        public ICommand AddInventoryProcessorCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryProcessorAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryProcessorCommand => new DelegateCommand<Inventory_numbers_processors>(inventoryProcessor =>
        {
            var editWindow = new InventoryProcessorEditWindow();
            var editViewModel = new InventoryProcessorEditViewModel(inventoryProcessor);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryProcessor => inventoryProcessor != null);

        public ICommand DeleteInventoryProcessorCommand => new DelegateCommand<Inventory_numbers_processors>(selectInventoryProcessor =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарный процессор:\nинвент. номер - {selectInventoryProcessor.Inventory_number};\nпроизводитель - {selectInventoryProcessor.Processor.Manufacturer.Name};\nнаименование - {selectInventoryProcessor.Processor.Name};\nсокет - {selectInventoryProcessor.Processor.Socket.Name};\nкол-во ядер - {selectInventoryProcessor.Processor.Amount_cores};\nбазовая частота - {selectInventoryProcessor.Processor.Base_frequency} {selectInventoryProcessor.Processor.Unit.Short_name}?", "Удаление инвентарного процессора", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_processors>(selectInventoryProcessor.Id_inventory_number_processor);
            RefreshCollection();
        }, selectInventoryProcessor => selectInventoryProcessor != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryProcessors.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_processors.AsNoTracking()
                .Include(processor => processor.Processor)
                .Include(manufacturer => manufacturer.Processor.Manufacturer)
                .Include(unit => unit.Processor.Unit)
                .Include(socket => socket.Processor.Socket))
            {
                InventoryProcessors.Add(item);
            }
            InventoryProcessors.Sort(inventoryProcessor => inventoryProcessor.Inventory_number);
        }
    }
}
