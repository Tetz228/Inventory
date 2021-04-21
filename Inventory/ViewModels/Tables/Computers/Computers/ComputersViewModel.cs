namespace Inventory.ViewModels.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.Computers.Computers;
    using Inventory.View.Edit.Tables.Computers.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers.Computers;

    public class ComputersViewModel : BindableBase
    {
        public ComputersViewModel()
        {
            using var db = new InventoryEntities();

            Computers = new ObservableCollection<Computer>(db.Computers.AsNoTracking().Include(status => status.Statuses_computers));
            Computers.Sort(inventoryPowerSupplies => inventoryPowerSupplies.Inventory_number, SortDirection = ListSortDirection.Ascending);
            ComputersCollection = CollectionViewSource.GetDefaultView(Computers);
        }

        #region Свойства
        private ICollectionView ComputersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Computer SelectComputer { get; set; }

        public static ObservableCollection<Computer> Computers { get; set; }

        private string _computersFilter = string.Empty;

        public string ComputersFilter
        {
            get => _computersFilter;
            set
            {
                _computersFilter = value;
                ComputersCollection.Filter = obj =>
                {
                    if (obj is Computer computer)
                        return computer.Search(ComputersFilter);

                    return false;
                };
                ComputersCollection.Refresh();
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
                    case "Инвентарный номер":
                        {
                            Computers.Sort(inventoryNumber => inventoryNumber.Inventory_number, SortDirection);
                            break;
                        }
                    case "IP-адрес":
                        {
                            Computers.Sort(computer => computer.Ip_address, SortDirection);
                            break;
                        }
                    case "Статус":
                        {
                            Computers.Sort(status => status.Statuses_computers.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectComputer = null;
        #endregion

        #region Команды
        public ICommand AddComputerCommand => new DelegateCommand(() =>
        {
            var addWindow = new ComputerAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditComputerCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            //var editWindow = new ComputerEditWindow();
            //var editViewModel = new ComputerEditViewModel(selectComputer);
            //editWindow.DataContext = editViewModel;
            //editWindow.Closing += editViewModel.OnWindowClosing;
            //editWindow.ShowDialog();
        }, computer => computer != null);

        public ICommand DeleteComputerCommand => new DelegateCommand<Computer>(selectComputer =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectComputer.Inventory_number} {selectComputer.Ip_address} {selectComputer.Statuses_computers.Name} ?", "Удаление компьютера", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Computer>(selectComputer.Id_computer);
            RefreshCollection();
        }, selectComputer => selectComputer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Computers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Computers.AsNoTracking().Include(status => status.Statuses_computers))
                Computers.Add(item);
        }
    }
}
