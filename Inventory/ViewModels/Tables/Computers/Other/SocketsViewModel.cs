namespace Inventory.ViewModels.Tables.Computers.Other
{
    using ClosedXML.Report;
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Model.Classes;

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
                        return socket.Search(SocketsFilter);

                    return false;
                };
                SocketsCollection.Refresh();
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
                                Sockets.Sort(socket => socket.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Sockets.Sort(socket => socket.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectSocket = null;
        #endregion

        #region Команды
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectSocket.Name}?", "Удаление сокета", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Socket>(selectSocket.Id_socket);
            RefreshCollection();
        }, selectSocket => selectSocket != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        public ICommand ExportExcelCommand => new DelegateCommand<ListView>(list =>
        {
            const string outputFile = @"D:\SocketReport.xlsx";
            var template = new XLTemplate(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Templates" + "\\OneColumnTemplate.xlsx");

            using var db = new InventoryEntities();

            var socket = db.Sockets.Select(name => name.Name);

            template.AddVariable("TableName", "Сокеты");
            template.AddVariable("Name", socket);
            template.Generate();
            template.SaveAs(outputFile);

            Process.Start(new ProcessStartInfo(outputFile) { UseShellExecute = true });
        });
        #endregion

        public static void RefreshCollection()
        {
            Sockets.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Sockets)
                Sockets.Add(item);
        }
    }
}
