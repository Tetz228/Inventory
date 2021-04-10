namespace Inventory.ViewModels.Tables.Computers.Other
{
    using ClosedXML.Report;
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class UnitsViewModel : BindableBase
    {
        public UnitsViewModel()
        {
            using var db = new InventoryEntities();

            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectUnit.Full_name}?", "Удаление единицы измерения", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Unit>(selectUnit.Id_unit);
            RefreshCollection();
        }, selectUnit => selectUnit != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        public ICommand ExportExcelCommand => new DelegateCommand<ListView>(list =>
        {
            string fileName = Services.IsSavingDocumentExcel();

            if (fileName != null)
            {
                var template = new XLTemplate(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Templates" + "\\TwoColumnTemplate.xlsx");

                using var db = new InventoryEntities();

                var unitFullName = db.Units.Select(name => name.Full_name);
                var unitShortName = db.Units.Select(name => name.Short_name);

                template.AddVariable("TableName", "Единицы измерения");
                template.AddVariable("ColumnName1", "Полное наименование");
                template.AddVariable("ColumnName2", "Краткое наименование");
                template.AddVariable("Name1", unitFullName);
                template.AddVariable("Name2", unitShortName);
                template.Generate();

                template.SaveAs(fileName);

                Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            }
        });
        #endregion

        public static void RefreshCollection()
        {
            Units.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Units.AsNoTracking())
                Units.Add(item);
        }
    }
}
