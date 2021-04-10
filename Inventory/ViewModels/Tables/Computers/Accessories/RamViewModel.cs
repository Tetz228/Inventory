namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
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

    public class RamViewModel : BindableBase
    {
        public RamViewModel()
        {
            using var db = new InventoryEntities();

            Rams = new ObservableCollection<Ram>(db.Rams.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_memory));
            Rams.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            RamsCollection = CollectionViewSource.GetDefaultView(Rams);
        }

        #region Свойства
        private ICollectionView RamsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Ram SelectRam { get; set; }

        public static ObservableCollection<Ram> Rams { get; set; }

        private string _ramsFilter = string.Empty;

        public string RamsFilter
        {
            get => _ramsFilter;
            set
            {
                _ramsFilter = value;
                RamsCollection.Filter = obj =>
                {
                    if (obj is Ram ram)
                        return ram.Search(RamsFilter);

                    return false;
                };
                RamsCollection.Refresh();
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
                            Rams.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Rams.Sort(ram => ram.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Rams.Sort(memory => memory.Memory_size, SortDirection);
                            break;
                        }
                    case "Единица измерения":
                        {
                            Rams.Sort(unit => unit.Unit.Full_name, SortDirection);
                            break;
                        }
                    case "Тактовая частота":
                        {
                            Rams.Sort(clock => clock.Clock_frequency, SortDirection);
                            break;
                        }
                    case "Тип памяти":
                        {
                            Rams.Sort(type => type.Types_memory.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectRam = null;
        #endregion

        #region Команды
        public ICommand AddRamCommand => new DelegateCommand(() =>
        {
            var addWindow = new RamAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditProcessorCommand => new DelegateCommand<Ram>(ram =>
        {
            var editWindow = new RamEditWindow();
            var editViewModel = new RamEditViewModel(ram);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, ram => ram != null);

        public ICommand DeleteRamCommand => new DelegateCommand<Ram>(selectRam =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectRam.Manufacturer.Name} {selectRam.Name} {selectRam.Memory_size} {selectRam.Unit.Short_name}?", "Удаление оперативной памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Ram>(selectRam.Id_ram);
            RefreshCollection();
        }, selectRam => selectRam != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Rams.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Rams.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_memory))
                Rams.Add(item);
        }
    }
}
