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

    public class TypesHddsViewModel : BindableBase
    {
        public TypesHddsViewModel()
        {
            using var db = new InventoryEntities();

            TypesHdds = new ObservableCollection<Types_hdd>(db.Types_hdd);
            TypesHdds.Sort(typeHdd => typeHdd.Name, SortDirection = ListSortDirection.Ascending);
            TypesHddsCollection = CollectionViewSource.GetDefaultView(TypesHdds);
        }

        #region Свойства

        private ICollectionView TypesHddsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Types_hdd> TypesHdds { get; set; }

        public Types_hdd SelectTypeHdd { get; set; }

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
                        return Types_hdd.SearchFor(typesHdd, TypesHddsFilter);

                    return false;
                };
                TypesHddsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                TypesHdds.Sort(typeHdd => typeHdd.Name, SortDirection = ListSortDirection.Descending);
                            else
                                TypesHdds.Sort(typeHdd => typeHdd.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }
        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectTypeHdd = null;
        #endregion

        #region Команды
        public ICommand AddTypeHddCommand => new DelegateCommand(() =>
        {
            var addWindow = new TypeHddAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditTypeHddCommand => new DelegateCommand<Types_hdd>(typeHdd =>
        {
            var editWindow = new TypeHddEditWindow();
            var editViewModel = new TypeHddEditViewModel(typeHdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        },
            typeHdd => typeHdd != null);

        public ICommand DeleteTypeHddCommand => new DelegateCommand<Types_hdd>(Types_hdd.DeleteTypeHdd, selectTypeHdd => selectTypeHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Types_hdd.RefreshCollection);
        #endregion
    }
}
