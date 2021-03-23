namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;
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
            using var db = new InventoryEntities();

            Units = new ObservableCollection<Unit>(db.Units);
            Units.Sort(unit => unit.Full_name, SortDirection = ListSortDirection.Ascending);
            UnitsCollection = CollectionViewSource.GetDefaultView(Units);
        }

        #region Свойства

        private ICollectionView UnitsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Unit> Units { get; set; }

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
                        return Unit.SearchFor(unit, UnitsFilter);

                    return false;
                };
                UnitsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Полное наименование":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Units.Sort(unit => unit.Full_name, SortDirection = ListSortDirection.Descending);
                        else
                            Units.Sort(unit => unit.Full_name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
                case "Краткое наименование":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Units.Sort(unit => unit.Short_name, SortDirection = ListSortDirection.Descending);
                        else
                            Units.Sort(unit => unit.Short_name, SortDirection = ListSortDirection.Ascending);
                        break;
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
        }, socket => socket != null);

        public ICommand DeleteUnitCommand => new DelegateCommand<Unit>(Unit.DeleteUnit, selectUnit => selectUnit != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Unit.RefreshCollection);
        #endregion
    }
}
