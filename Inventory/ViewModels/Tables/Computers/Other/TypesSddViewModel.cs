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

    public class TypesSddViewModel : BindableBase
    {
        public TypesSddViewModel()
        {
            RefreshCollection();
            TypesSsdCollection = CollectionViewSource.GetDefaultView(TypesSsd);
        }

        #region Свойства

        private ICollectionView TypesSsdCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Types_ssd> TypesSsd { get; set; } = new();

        public Types_ssd SelectTypeSsd { get; set; }

        private string _typesSsdFilter = string.Empty;

        public string TypesSsdFilter
        {
            get => _typesSsdFilter;
            set
            {
                _typesSsdFilter = value;
                TypesSsdCollection.Filter = obj =>
                {
                    if (obj is Types_ssd typeSsd)
                        return typeSsd.Search(TypesSsdFilter);

                    return false;
                };
                TypesSsdCollection.Refresh();
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
                            TypesSsd.Sort(typeSsd => typeSsd.Name, SortDirection);
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить тип:\nнаименование - {selectTypeSsd.Name}?", "Удаление типа SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Types_ssd>(selectTypeSsd.Id_type_ssd);
            TypesSsd.Remove(selectTypeSsd);
        }, selectTypeSsd => selectTypeSsd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        private static void RefreshCollection()
        {
            TypesSsd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_ssd.AsNoTracking())
            {
                TypesSsd.Add(item);
            }
            TypesSsd.Sort(typeSsd => typeSsd.Name);
        }
    }
}
