namespace Inventory.ViewModels.Tables.Computers.Computers
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
    using Inventory.View.Add.Tables.Computers.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers.Computers;

    public class StatusesComputersViewModel : BindableBase
    {
        public StatusesComputersViewModel()
        {
            using var db = new InventoryEntities();

            StatusesComputers = new ObservableCollection<Statuses_computers>(db.Statuses_computers);
            StatusesComputers.Sort(statusComputer => statusComputer.Name, SortDirection = ListSortDirection.Ascending);
            StatusesComputersCollection = CollectionViewSource.GetDefaultView(StatusesComputers);
        }

        #region Свойства

        private ICollectionView StatusesComputersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Statuses_computers> StatusesComputers { get; set; }

        public Statuses_computers SelectStatusComputer { get; set; }

        private string _statusesComputersFilter = string.Empty;

        public string StatusesComputersFilter
        {
            get => _statusesComputersFilter;
            set
            {
                _statusesComputersFilter = value;
                StatusesComputersCollection.Filter = obj =>
                {
                    if (obj is Statuses_computers statusComputer)
                        return Statuses_computers.SearchFor(statusComputer, StatusesComputersFilter);

                    return false;
                };
                StatusesComputersCollection.Refresh();
            }
        }
        #endregion

        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                StatusesComputers.Sort(statusComputer => statusComputer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                StatusesComputers.Sort(statusComputer => statusComputer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectStatusComputer = null;

        #region Команды
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

        public ICommand DeleteStatusComputerCommand => new DelegateCommand<Statuses_computers>(Statuses_computers.DeleteStatusComputer, selectStatusComputer => selectStatusComputer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Statuses_computers.RefreshCollection);
        #endregion
    }
}
