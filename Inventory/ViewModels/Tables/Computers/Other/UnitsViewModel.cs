namespace Inventory.ViewModels.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class UnitsViewModel : BaseViewModel<Unit>
    {
        public UnitsViewModel() : base(Units) => RefreshCollection();

        public static ObservableCollection<Unit> Units { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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

            if (Services.Delete<Unit>(selectUnit.Id_unit))
                Units.Remove(selectUnit);
        }, selectUnit => selectUnit != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        private static void RefreshCollection()
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
