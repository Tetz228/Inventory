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

    public class TypesHddViewModel : BaseViewModel<Types_hdd>
    {
        public TypesHddViewModel() : base(TypesHdd) => RefreshCollection();

        #region Свойства

        public static ObservableCollection<Types_hdd> TypesHdd { get; set; } = new();

        private string _typesHddFilter = string.Empty;

        public string TypesHddFilter
        {
            get => _typesHddFilter;
            set
            {
                _typesHddFilter = value;
                CollectionView.Filter = obj =>
                {
                    if (obj is Types_hdd typesHdd)
                        return typesHdd.Search(TypesHddFilter);

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
                            TypesHdd.Sort(typeHdd => typeHdd.Name, SortDirection);
                            break;
                        }
                }
            }
        }

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
        }, typeHdd => typeHdd != null);

        public ICommand DeleteTypeHddCommand => new DelegateCommand<Types_hdd>(selectTypeHdd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить тип:\nнаименование - {selectTypeHdd.Name}?", "Удаление типа жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Types_hdd>(selectTypeHdd.Id_type_hdd))
                TypesHdd.Remove(selectTypeHdd);
        }, selectTypeHdd => selectTypeHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        private static void RefreshCollection()
        {
            TypesHdd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_hdd.AsNoTracking())
            {
                TypesHdd.Add(item);
            }
            TypesHdd.Sort(typeHdd => typeHdd.Name);
        }
    }
}
