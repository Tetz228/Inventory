namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;

    class TypesHddViewModel : BindableBase
    {
        public TypesHddViewModel()
        {
            using var db = new InventoryEntities();

            TypesHdds = new ObservableCollection<Types_hdd>(db.Types_hdd);
            TypesHddsCollection = CollectionViewSource.GetDefaultView(TypesHdds);
            TypesHddsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Types_hdd> TypesHdds { get; set; }

        public ICollectionView TypesHddsCollection { get; }

        public Types_hdd SelectTypeHdd 
        { get; 
            set; }

        private string _typesHddsFilter = string.Empty;

        public string TypesHddsFilter
        {
            get => _typesHddsFilter;
            set
            {
                _typesHddsFilter = value;
                TypesHddsCollection.Filter = obj =>
                {
                    if (obj is Types_hdd typesHdd)
                        return Types_hdd.Search(typesHdd, TypesHddsFilter);

                    return false;
                };
                TypesHddsCollection.Refresh();
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
                        if (TypesHddsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            TypesHddsCollection.SortDescriptions.Clear();
                            TypesHddsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            TypesHddsCollection.SortDescriptions.Clear();
                            TypesHddsCollection.SortDescriptions.Add(new SortDescription(nameof(Types_peripherals.Name), ListSortDirection.Ascending));
                        }
                        TypesHddsCollection.Refresh();
                        break;
                    }
            }
        }
        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectTypeHdd = null;
        #endregion

        #region Команды
        public ICommand AddTypeHdd => new DelegateCommand(() =>
        {
            var addWindow = new TypeHddAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditTypeHdd => new DelegateCommand<Types_hdd>(typeHdd =>
        {
            var editWindow = new TypeHddEditWindow();
            var editViewModel = new TypeHddEditViewModel(typeHdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, 
            typeHdd => typeHdd != null);

        public ICommand DeleteTypeHdd => new DelegateCommand<Types_hdd>(Types_hdd.DeleteTypeHdd, selectTypeHdd => selectTypeHdd != null);

        public ICommand RefreshList => new DelegateCommand(Types_hdd.RefreshCollection);
        #endregion
    }
}
