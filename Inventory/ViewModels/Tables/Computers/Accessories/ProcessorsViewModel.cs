namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Services;

    public class ProcessorsViewModel : BindableBase
    {
        public ProcessorsViewModel()
        {
            using var db = new InventoryEntities();

            Processors = new ObservableCollection<Processor>(db.Processors.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(socket => socket.Socket));
            Processors.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            ProcessorsCollection = CollectionViewSource.GetDefaultView(Processors);
        }

        #region Свойства
        private ICollectionView ProcessorsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Processor SelectProcessor { get; set; }

        public static ObservableCollection<Processor> Processors { get; set; }

        private string _processorsFilter = string.Empty;

        public string ProcessorsFilter
        {
            get => _processorsFilter;
            set
            {
                _processorsFilter = value;
                ProcessorsCollection.Filter = obj =>
                {
                    if (obj is Processor processor)
                        return processor.Search(ProcessorsFilter);

                    return false;
                };
                ProcessorsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(powerSupply => powerSupply.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(powerSupply => powerSupply.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Сокет":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(powerSupply => powerSupply.Socket.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(powerSupply => powerSupply.Socket.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Количество ядер":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(powerSupply => powerSupply.Amount_cores, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(powerSupply => powerSupply.Amount_cores, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Базовая частота":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(powerSupply => powerSupply.Base_frequency, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(powerSupply => powerSupply.Base_frequency, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Единица измерения":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Processors.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                            else
                                Processors.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectProcessor = null;
        #endregion

        #region Команды
        public ICommand AddProcessorCommand => new DelegateCommand(() =>
        {
            var addWindow = new ProcessorAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditProcessorCommand => new DelegateCommand<Processor>(processor =>
        {
            var editWindow = new ProcessorEditWindow();
            var editViewModel = new ProcessorEditViewModel(processor);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, processor => processor != null);

        public ICommand DeleteProcessorCommand => new DelegateCommand<Processor>(selectProcessor =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectProcessor.Manufacturer.Name} {selectProcessor.Name} {selectProcessor.Amount_cores} {selectProcessor.Unit.Short_name}?", "Удаление процессора", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Processor>(selectProcessor.Id_processor);
            RefreshCollection();
        }, selectProcessor => selectProcessor != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Processors.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Processors.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(socket => socket.Socket))
                Processors.Add(item);
        }
    }
}
