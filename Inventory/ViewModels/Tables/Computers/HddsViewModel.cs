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

    class HddsViewModel:BindableBase
    {
        public HddsViewModel()
        {
            using var db = new InventoryEntities();

            Hdds = new ObservableCollection<Hdd>(db.Hdds);
            HddsCollection = CollectionViewSource.GetDefaultView(Hdds);
            HddsCollection.SortDescriptions.Add(new SortDescription(nameof(Hdd.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Hdd> Hdds { get; set; }

        public ICollectionView HddsCollection { get; }

        public Hdd SelectHdd { get; set; }

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
                        return Hdd.Search(hdd, HddsFilter);

                    return false;
                };
                HddsCollection.Refresh();
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
                case "Наименование":
                    {
                        if (HddsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            HddsCollection.SortDescriptions.Clear();
                            HddsCollection.SortDescriptions.Add(new SortDescription(nameof(Hdd.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            HddsCollection.SortDescriptions.Clear();
                            HddsCollection.SortDescriptions.Add(new SortDescription(nameof(Hdd.Name), ListSortDirection.Ascending));
                        }
                        HddsCollection.Refresh();
                        break;
                    }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectHdd = null;
        #endregion

        #region Команды
        public ICommand AddHdd => new DelegateCommand(() =>
        {
            var addHddWindow = new HddAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditHdd => new DelegateCommand<Hdd>(hdd =>
        {
            var editWindow = new HddEditWindow();
            var editViewModel = new HddEditViewModel(hdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, hdd => hdd != null);

        public ICommand DeleteHdd => new DelegateCommand<Hdd>(Hdd.DeleteHdd, selectHdd => selectHdd != null);

        public ICommand RefreshList => new DelegateCommand(Hdd.RefreshCollection);
        #endregion
    }
}
