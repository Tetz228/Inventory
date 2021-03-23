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

    public class TypesSddsViewModel : BindableBase
    {
        public TypesSddsViewModel()
        {
            using var db = new InventoryEntities();

            TypesSsds = new ObservableCollection<Types_ssd>(db.Types_ssd);
            TypesSsds.Sort(typeSsd => typeSsd.Name, SortDirection = ListSortDirection.Ascending);
            TypesSsdsCollection = CollectionViewSource.GetDefaultView(TypesSsds);
        }

        #region Свойства

        private ICollectionView TypesSsdsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Types_ssd> TypesSsds { get; set; }

        public Types_ssd SelectTypeSsd { get; set; }

        private string _typesSsdsFilter = string.Empty;

        public string TypesSsdsFilter
        {
            get => _typesSsdsFilter;
            set
            {
                _typesSsdsFilter = value;
                TypesSsdsCollection.Filter = obj =>
                {
                    if (obj is Types_ssd typeSsd)
                        return Types_ssd.SearchFor(typeSsd, TypesSsdsFilter);

                    return false;
                };
                TypesSsdsCollection.Refresh();
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
                case "Наименование":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            TypesSsds.Sort(typeSsd => typeSsd.Name, SortDirection = ListSortDirection.Descending);
                        else
                            TypesSsds.Sort(typeSsd => typeSsd.Name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
            }
        }
        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectTypeSsd = null;
        #endregion

        #region Команды
        public ICommand AddTypeSsdCommand => new DelegateCommand(() =>
        {
            var addWindow = new TypeSsdAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditTypeSsdCommand => new DelegateCommand<Types_ssd>(typeSsd =>
        {
            var editWindow = new TypeSsdEditWindow();
            var editViewModel = new TypeSsdEditViewModel(typeSsd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();

        }, typeSsd => typeSsd != null);

        public ICommand DeleteTypeSsdCommand => new DelegateCommand<Types_ssd>(Types_ssd.DeleteTypeSsd, selectTypeSsd => selectTypeSsd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Types_ssd.RefreshCollection);
        #endregion
    }
}
