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

    public class TypesSddViewModel : BaseViewModel<Types_ssd>
    {
        public TypesSddViewModel() : base(TypesSsd) => RefreshCollection();

        #region Свойства

        public static ObservableCollection<Types_ssd> TypesSsd { get; set; } = new();

        private string _typesSsdFilter = string.Empty;

        public string TypesSsdFilter
        {
            get => _typesSsdFilter;
            set
            {
                _typesSsdFilter = value;
                CollectionView.Filter = obj =>
                {
                    if (obj is Types_ssd typeSsd)
                        return typeSsd.Search(TypesSsdFilter);

                    return false;
                };
                CollectionView.Refresh();
            }
        }
        #endregion

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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

            if (Services.Delete<Types_ssd>(selectTypeSsd.Id_type_ssd))
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
