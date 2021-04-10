namespace Inventory.ViewModels.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class TypesMemoryViewModel : BindableBase
    {
        public TypesMemoryViewModel()
        {
            using var db = new InventoryEntities();

            TypesMemory = new ObservableCollection<Types_memory>(db.Types_memory.AsNoTracking());
            TypesMemory.Sort(typeMemory => typeMemory.Name, SortDirection = ListSortDirection.Ascending);
            TypesMemoryCollection = CollectionViewSource.GetDefaultView(TypesMemory);
        }

        #region Свойства

        private ICollectionView TypesMemoryCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Types_memory> TypesMemory { get; set; }

        public Types_memory SelectTypeMemory { get; set; }

        private string _typesMemoryFilter = string.Empty;

        public string TypesMemoryFilter
        {
            get => _typesMemoryFilter;
            set
            {
                _typesMemoryFilter = value;
                TypesMemoryCollection.Filter = obj =>
                {
                    if (obj is Types_memory typeMemory)
                        return typeMemory.Search(TypesMemoryFilter);

                    return false;
                };
                TypesMemoryCollection.Refresh();
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
                            TypesMemory.Sort(typeMemory => typeMemory.Name, SortDirection);
                            break;
                        }
                }
            }
        }
        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectTypeMemory = null;
        #endregion

        #region Команды
        public ICommand AddTypeMemoryCommand => new DelegateCommand(() =>
        {
            var addWindow = new TypeMemoryAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditTypeMemoryCommand => new DelegateCommand<Types_memory>(typeMemory =>
        {
            var editWindow = new TypeMemoryEditWindow();
            var editViewModel = new TypeMemoryEditViewModel(typeMemory);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();

        }, typeSsd => typeSsd != null);

        public ICommand DeleteTypeMemoryCommand => new DelegateCommand<Types_memory>(selectTypeMemory =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectTypeMemory.Name}?", "Удаление типа памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Types_memory>(selectTypeMemory.Id_type_memory);
            RefreshCollection();
        }, selectTypeMemory => selectTypeMemory != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            TypesMemory.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_memory.AsNoTracking())
                TypesMemory.Add(item);
        }
    }
}
