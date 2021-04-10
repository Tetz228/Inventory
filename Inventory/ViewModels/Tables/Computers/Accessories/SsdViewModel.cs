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

    public class SsdViewModel : BindableBase
    {
        public SsdViewModel()
        {
            using var db = new InventoryEntities();

            Ssds = new ObservableCollection<Ssd>(db.Ssds.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_ssd));
            Ssds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
            SsdsCollection = CollectionViewSource.GetDefaultView(Ssds);
        }

        #region Свойства
        private ICollectionView SsdsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Ssd SelectSsd { get; set; }

        public static ObservableCollection<Ssd> Ssds { get; set; }

        private string _ssdsFilter = string.Empty;

        public string SsdsFilter
        {
            get => _ssdsFilter;
            set
            {
                _ssdsFilter = value;
                SsdsCollection.Filter = obj =>
                {
                    if (obj is Ssd ssd)
                        return ssd.Search(SsdsFilter);

                    return false;
                };
                SsdsCollection.Refresh();
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
                            Ssds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            Ssds.Sort(type => type.Types_ssd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Ssds.Sort(ssd => ssd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Ssds.Sort(ssd => ssd.Memory_size, SortDirection);
                            break;
                        }
                    case "Единица измерения":
                        {
                            Ssds.Sort(unit => unit.Unit.Full_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectSsd = null;
        #endregion

        #region Команды
        public ICommand AddSsdCommand => new DelegateCommand(() =>
        {
            var addSsdWindow = new SsdAddWindow();
            addSsdWindow.ShowDialog();
        });

        public ICommand EditSsdCommand => new DelegateCommand<Ssd>(ssd =>
        {
            var editWindow = new SsdEditWindow();
            var editViewModel = new SsdEditViewModel(ssd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, ssd => ssd != null);

        public ICommand DeleteSsdCommand => new DelegateCommand<Ssd>(selectSsd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectSsd.Manufacturer.Name} {selectSsd.Name} {selectSsd.Memory_size} {selectSsd.Unit.Short_name}?", "Удаление SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Ssd>(selectSsd.Id_ssd);
            RefreshCollection();
        }, selectSsd => selectSsd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Ssds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Ssds.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_ssd))
                Ssds.Add(item);
        }
    }
}
