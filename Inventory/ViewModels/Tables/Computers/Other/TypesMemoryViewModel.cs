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

    public class TypesMemoryViewModel : BaseViewModel<Types_memory>
    {
        public TypesMemoryViewModel() : base(TypesMemory) => RefreshCollection();

        public static ObservableCollection<Types_memory> TypesMemory { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить тип:\nнаименование - {selectTypeMemory.Name}?", "Удаление типа памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Types_memory>(selectTypeMemory.Id_type_memory))
                TypesMemory.Remove(selectTypeMemory);
        }, selectTypeMemory => selectTypeMemory != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        private static void RefreshCollection()
        {
            TypesMemory.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_memory.AsNoTracking())
            {
                TypesMemory.Add(item);
            }
            TypesMemory.Sort(typeMemory => typeMemory.Name);
        }
    }
}
