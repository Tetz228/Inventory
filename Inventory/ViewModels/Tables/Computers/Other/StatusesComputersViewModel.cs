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

    public class StatusesComputersViewModel : BaseViewModel<Statuses_computers>
    {
        public StatusesComputersViewModel() : base(StatusesComputers) => RefreshCollection();

        public static ObservableCollection<Statuses_computers> StatusesComputers { get; set; } = new();
        
        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            StatusesComputers.Sort(statusComputer => statusComputer.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddStatusComputerCommand => new DelegateCommand(() =>
        {
            var addWindow = new StatusComputerAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditStatusComputerCommand => new DelegateCommand<Statuses_computers>(statusComputer =>
        {
            var editWindow = new StatusComputerEditWindow();
            var editViewModel = new StatusComputerEditViewModel(statusComputer);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, statusComputer => statusComputer != null);

        public ICommand DeleteStatusComputerCommand => new DelegateCommand<Statuses_computers>(selectStatusComputer =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить статус:\nнаименование - {selectStatusComputer.Name}?", "Удаление статуса компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Statuses_computers>(selectStatusComputer.Id_status_computer))
                StatusesComputers.Remove(selectStatusComputer);
        }, selectStatusComputer => selectStatusComputer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
       
        private static void RefreshCollection()
        {
            StatusesComputers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_computers.AsNoTracking())
                StatusesComputers.Add(item);

            StatusesComputers.Sort(statusComputer => statusComputer.Name);
        }
    }
}
