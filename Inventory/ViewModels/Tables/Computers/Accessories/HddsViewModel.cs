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

    public class HddsViewModel : BindableBase
    {
        public HddsViewModel()
        {
            RefreshCollection();
            HddsCollection = CollectionViewSource.GetDefaultView(Hdds);
        }

        #region Свойства
        private ICollectionView HddsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Hdd SelectHdd { get; set; }

        public static ObservableCollection<Hdd> Hdds { get; set; } = new();

        private string _hddsFilter = string.Empty;

        public string HddsFilter
        {
            get => _hddsFilter;
            set
            {
                _hddsFilter = value;
                HddsCollection.Filter = obj =>
                {
                    if (obj is Hdd hdd)
                        return hdd.Search(HddsFilter);

                    return false;
                };
                HddsCollection.Refresh();
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
                            Hdds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            Hdds.Sort(type => type.Types_hdd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Hdds.Sort(hdd => hdd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Hdds.Sort(hdd => hdd.Memory_size + " " + hdd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectHdd = null;
        #endregion

        #region Команды
        public ICommand AddHddCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new HddAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditHddCommand => new DelegateCommand<Hdd>(hdd =>
        {
            var editWindow = new HddEditWindow();
            var editViewModel = new HddEditViewModel(hdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, hdd => hdd != null);

        public ICommand DeleteHddCommand => new DelegateCommand<Hdd>(selectHdd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить жесткий диск:\nпроизводитель - {selectHdd.Manufacturer.Name};\nтип - {selectHdd.Types_hdd.Name};\nнаименование - {selectHdd.Name};\nобъём - {selectHdd.Memory_size} {selectHdd.Unit.Short_name}?", "Удаление жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Hdd>(selectHdd.Id_hdd);
            Hdds.Remove(selectHdd);
        }, selectHdd => selectHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Hdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Hdds.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_hdd))
                Hdds.Add(item);

            Hdds.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
