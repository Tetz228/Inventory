namespace Inventory.ViewModels.Tables.Computers.Other
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
    using Inventory.ViewModels.Edit.Tables.Computers.Other;

    public class TypesMemoryViewModel : BindableBase
    {
        public TypesMemoryViewModel()
        {
            using var db = new InventoryEntities();

            TypesMemory = new ObservableCollection<Types_memory>(db.Types_memory);
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
                        return Types_memory.SearchFor(typeMemory, TypesMemoryFilter);

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
                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                TypesMemory.Sort(typeMemory => typeMemory.Name, SortDirection = ListSortDirection.Descending);
                            else
                                TypesMemory.Sort(typeMemory => typeMemory.Name, SortDirection = ListSortDirection.Ascending);
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

        public ICommand DeleteTypeMemoryCommand => new DelegateCommand<Types_memory>(Types_memory.DeleteTypeMemory, selectTypeMemory => selectTypeMemory != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Types_memory.RefreshCollection);
        #endregion
    }
}
