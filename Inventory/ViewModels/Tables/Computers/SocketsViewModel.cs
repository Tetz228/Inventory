namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class SocketsViewModel : BindableBase
    {
        public SocketsViewModel()
        {
            using var db = new InventoryEntities();

            Sockets = new ObservableCollection<Socket>(db.Sockets);
            Sockets.Sort(socket => socket.Name, SortDirection = ListSortDirection.Ascending);
            SocketsCollection = CollectionViewSource.GetDefaultView(Sockets);
        }

        #region Свойства
        private ICollectionView SocketsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Socket> Sockets { get; set; }

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
                        if (SortDirection == ListSortDirection.Ascending)
                            Sockets.Sort(socket => socket.Name, SortDirection = ListSortDirection.Descending);
                        else
                            Sockets.Sort(socket => socket.Name, SortDirection = ListSortDirection.Ascending);
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
