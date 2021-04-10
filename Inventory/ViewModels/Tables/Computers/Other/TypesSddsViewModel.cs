namespace Inventory.ViewModels.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Services;

    public class TypesSddsViewModel : BindableBase
    {
        public TypesSddsViewModel()
        {
            using var db = new InventoryEntities();

            TypesSsds = new ObservableCollection<Types_ssd>(db.Types_ssd.AsNoTracking());
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
                        return typeSsd.Search(TypesSsdsFilter);

                    return false;
                };
                TypesSsdsCollection.Refresh();
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
                    case "Наименование":
                        {
                            TypesSsds.Sort(typeSsd => typeSsd.Name, SortDirection);
                            break;
                        }
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

        public ICommand DeleteTypeSsdCommand => new DelegateCommand<Types_ssd>(selectTypeSsd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectTypeSsd.Name}?", "Удаление типа SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Types_ssd>(selectTypeSsd.Id_type_ssd);
            RefreshCollection();
        }, selectTypeSsd => selectTypeSsd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            TypesSsds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_ssd.AsNoTracking())
                TypesSsds.Add(item);
        }
    }
}
