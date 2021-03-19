namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Computers;

    public class SocketsViewModel : BindableBase
    {
        public SocketsViewModel()
        {
            using var db = new InventoryEntities();

            Sockets = new ObservableCollection<Socket>(db.Sockets);
            SocketsCollection = CollectionViewSource.GetDefaultView(Sockets);
            SocketsCollection.SortDescriptions.Add(new SortDescription(nameof(Socket.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Socket> Sockets { get; set; }

        public ICollectionView SocketsCollection { get; }

        public Socket SelectSocket { get; set; }

        private string _socketsFilter = string.Empty;

        public string SocketsFilter
        {
            get => _socketsFilter;
            set
            {
                _socketsFilter = value;
                SocketsCollection.Filter = obj =>
                {
                    if (obj is Socket socket)
                        return Socket.Search(socket, SocketsFilter);

                    return false;
                };
                SocketsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                    {
                        if (SocketsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            SocketsCollection.SortDescriptions.Clear();
                            SocketsCollection.SortDescriptions.Add(new SortDescription(nameof(Socket.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            SocketsCollection.SortDescriptions.Clear();
                            SocketsCollection.SortDescriptions.Add(new SortDescription(nameof(Socket.Name), ListSortDirection.Ascending));
                        }
                        SocketsCollection.Refresh();
                        break;
                    }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectSocket = null;
        #endregion

        #region Команды
        public ICommand AddSocket => new DelegateCommand(() =>
        {
            var addPostWindow = new SocketAddWindow();
            addPostWindow.ShowDialog();
        });

        public ICommand EditSocket => new DelegateCommand<Socket>(socket =>
        {
            var editWindow = new SocketEditWindow();
            var editViewModel = new SocketEditViewModel(socket);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, socket => socket != null);

        public ICommand DeleteSocket => new DelegateCommand<Socket>(Socket.DeleteSocket, selectSocket => selectSocket != null);

        public ICommand RefreshList => new DelegateCommand(Socket.RefreshCollection);
        #endregion
    }
}
