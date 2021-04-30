namespace Inventory.ViewModels.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class UnitsViewModel : BindableBase
    {
        public UnitsViewModel()
        {
            RefreshCollection();
            UnitsCollection = CollectionViewSource.GetDefaultView(Units);
        }

        #region Свойства

        private ICollectionView UnitsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Unit> Units { get; set; } = new();

        public Unit SelectUnit { get; set; }

        private string _unitsFilter = string.Empty;

        public string UnitsFilter
        {
            get => _unitsFilter;
            set
            {
                _unitsFilter = value;
                UnitsCollection.Filter = obj =>
                {
                    if (obj is Unit unit)
                        return unit.Search(UnitsFilter);

                    return false;
                };
                UnitsCollection.Refresh();
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
                    case "Полное наименование":
                        {
                            Units.Sort(unit => unit.Full_name, SortDirection);
                            break;
                        }
                    case "Краткое наименование":
                        {
                            Units.Sort(unit => unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectUnit = null;
        #endregion

        #region Команды
        public ICommand AddUnitCommand => new DelegateCommand(() =>
        {
            var addWindow = new UnitAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditUnitCommand => new DelegateCommand<Unit>(unit =>
        {
            var editWindow = new UnitEditWindow();
            var editViewModel = new UnitEditViewModel(unit);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, unit => unit != null);

        public ICommand DeleteUnitCommand => new DelegateCommand<Unit>(selectUnit =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить единицу измерения:\nполное наименование - {selectUnit.Full_name};\nкраткое наименование - {selectUnit.Short_name}?", "Удаление единицы измерения", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Unit>(selectUnit.Id_unit);
            RefreshCollection();
        }, selectUnit => selectUnit != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Units.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Units.AsNoTracking())
            {
                Units.Add(item);
            }
            Units.Sort(unit => unit.Full_name);
        }
    }
}
