namespace Inventory.ViewModels.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Computers;
    using Inventory.View.Edit.Tables.Computers.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class StatusesComputersViewModel : BindableBase
    {
        public StatusesComputersViewModel()
        {
            using var db = new InventoryEntities();

            StatusesComputers = new ObservableCollection<Statuses_computers>(db.Statuses_computers.AsNoTracking()).Sort(statusComputer => statusComputer.Name);
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
                        return statusComputer.Search(StatusesComputersFilter);

                    return false;
                };
                StatusesComputersCollection.Refresh();
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
                            StatusesComputers.Sort(statusComputer => statusComputer.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectStatusComputer = null;

        #endregion

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

        public ICommand DeleteStatusComputerCommand => new DelegateCommand<Statuses_computers>(selectStatusComputer =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить статус:\nнаименование - {selectStatusComputer.Name}?", "Удаление статуса компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Statuses_computers>(selectStatusComputer.Id_status_computer);
            RefreshCollection();
        }, selectStatusComputer => selectStatusComputer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            StatusesComputers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_computers.AsNoTracking())
                StatusesComputers.Add(item);

            StatusesComputers.Sort(statusComputer => statusComputer.Name);
        }
    }
}
