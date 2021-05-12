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

    public class SocketsViewModel : BaseViewModel<Socket>
    {
        public SocketsViewModel() : base(Sockets) => RefreshCollection();

        public static ObservableCollection<Socket> Sockets { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            Sockets.Sort(socket => socket.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddSocketCommand => new DelegateCommand(() =>
        {
            var addPostWindow = new SocketAddWindow();
            addPostWindow.ShowDialog();
        });

        public ICommand EditSocketCommand => new DelegateCommand<Socket>(socket =>
        {
            var editWindow = new SocketEditWindow();
            var editViewModel = new SocketEditViewModel(socket);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, socket => socket != null);

        public ICommand DeleteSocketCommand => new DelegateCommand<Socket>(selectSocket =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить сокет:\nнаименование - {selectSocket.Name}?", "Удаление сокета", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Socket>(selectSocket.Id_socket))
                Sockets.Remove(selectSocket);
        }, selectSocket => selectSocket != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        
        private static void RefreshCollection()
        {
            Sockets.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Sockets.AsNoTracking())
            {
                Sockets.Add(item);
            }
            Sockets.Sort(socket => socket.Name);
        }
    }
}
