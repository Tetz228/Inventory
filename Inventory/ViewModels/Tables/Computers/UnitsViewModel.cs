using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Tables.Computers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;

    class  UnitsViewModel : BindableBase
    {
        public UnitsViewModel()
        {
            using var db = new InventoryEntities();

            Units = new ObservableCollection<Unit>(db.Units);
            UnitsCollection = CollectionViewSource.GetDefaultView(Units);
            UnitsCollection.SortDescriptions.Add(new SortDescription(nameof(Unit.Short_name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Unit> Units { get; set; }

        public ICollectionView UnitsCollection { get; }

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
                        return Unit.Search(unit, UnitsFilter);

                    return false;
                };
                UnitsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Полное наименование":
                    {
                        if (UnitsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            UnitsCollection.SortDescriptions.Clear();
                            UnitsCollection.SortDescriptions.Add(new SortDescription(nameof(Unit.Full_name), ListSortDirection.Descending));
                        }
                        else
                        {
                            UnitsCollection.SortDescriptions.Clear();
                            UnitsCollection.SortDescriptions.Add(new SortDescription(nameof(Unit.Full_name), ListSortDirection.Ascending));
                        }
                        UnitsCollection.Refresh();
                        break;
                    }
                case "Краткое наименование":
                {
                    if (UnitsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                    {
                        UnitsCollection.SortDescriptions.Clear();
                        UnitsCollection.SortDescriptions.Add(new SortDescription(nameof(Unit.Short_name), ListSortDirection.Descending));
                    }
                    else
                    {
                        UnitsCollection.SortDescriptions.Clear();
                        UnitsCollection.SortDescriptions.Add(new SortDescription(nameof(Unit.Short_name), ListSortDirection.Ascending));
                    }
                    UnitsCollection.Refresh();
                    break;
                }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectUnit = null;
        #endregion

        #region Команды
        public ICommand AddUnit => new DelegateCommand(() =>
        {
            var addWindow = new UnitAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditUnit => new DelegateCommand<Unit>(unit =>
        {
            var editWindow = new UnitEditWindow();
            var editViewModel = new UnitEditViewModel(unit);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, socket => socket != null);

        public ICommand DeleteUnit => new DelegateCommand<Unit>(Unit.DeleteUnit, selectUnit => selectUnit != null);

        public ICommand RefreshList => new DelegateCommand(Unit.RefreshCollection);
        #endregion
    }
}
