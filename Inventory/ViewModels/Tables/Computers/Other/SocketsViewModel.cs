namespace Inventory.ViewModels.Tables.Computers.Other
{
    using System;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using ClosedXML.Excel;

    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;

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
                        return Socket.SearchFor(socket, SocketsFilter);

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

        public ICommand DeleteSocketCommand => new DelegateCommand<Socket>(Socket.DeleteSocket, selectSocket => selectSocket != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Socket.RefreshCollection);
        #endregion

        public ICommand ExportExcelCommand => new DelegateCommand(() =>
        {
            #region MyRegion

            /*
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Contacts");
            // Title
            ws.Cell("B2").Value = "Contacts";

            // First Names
            ws.Cell("B3").Value = "FName";
            ws.Cell("B4").Value = "John";
            ws.Cell("B5").Value = "Hank";
            ws.Cell("B6").SetValue("Dagny"); // Another way to set the value

            // Last Names
            ws.Cell("C3").Value = "LName";
            ws.Cell("C4").Value = "Galt";
            ws.Cell("C5").Value = "Rearden";
            ws.Cell("C6").SetValue("Taggart"); // Another way to set the value
            // Boolean
            ws.Cell("D3").Value = "Outcast";
            ws.Cell("D4").Value = true;
            ws.Cell("D5").Value = false;
            ws.Cell("D6").SetValue(false); // Another way to set the value

            // DateTime
            ws.Cell("E3").Value = "DOB";
            ws.Cell("E4").Value = new DateTime(1919, 1, 21);
            ws.Cell("E5").Value = new DateTime(1907, 3, 4);
            ws.Cell("E6").SetValue(new DateTime(1921, 12, 15)); // Another way to set the value

            // Numeric
            ws.Cell("F3").Value = "Income";
            ws.Cell("F4").Value = 2000;
            ws.Cell("F5").Value = 40000;
            ws.Cell("F6").SetValue(10000); // Another way to set the value

            // From worksheet
            var rngTable = ws.Range("B2:F6");

            // From another range
            var rngDates = rngTable.Range("D3:D5"); // The address is relative to rngTable (NOT the worksheet)
            var rngNumbers = rngTable.Range("E3:E5"); // The address is relative to rngTable (NOT the worksheet)

            // Using a OpenXML's predefined formats
            rngDates.Style.NumberFormat.NumberFormatId = 15;

            // Using a custom format
            rngNumbers.Style.NumberFormat.Format = "$ #,##0";

            rngTable.FirstCell().Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.CornflowerBlue)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            rngTable.FirstRow().Merge(); // We could've also used: rngTable.Range("A1:E1").Merge() or rngTable.Row(1).Merge()

            var rngHeaders = rngTable.Range("B2:E2"); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = XLColor.DarkBlue;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            var rngData = ws.Range("B3:F6");
            var excelTable = rngData.CreateTable();

            // Add the totals row
            excelTable.ShowTotalsRow = true;
            // Put the average on the field "Income"
            // Notice how we're calling the cell by the column name
            excelTable.Field("Income").TotalsRowFunction = XLTotalsRowFunction.Average;
            // Put a label on the totals cell of the field "DOB"
            excelTable.Field("DOB").TotalsRowLabel = "Average:";

            // Add thick borders to the contents of our spreadsheet
            ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            // You can also specify the border for each side:
            // contents.FirstColumn().Style.Border.LeftBorder = XLBorderStyleValues.Thick;
            // contents.LastColumn().Style.Border.RightBorder = XLBorderStyleValues.Thick;
            // contents.FirstRow().Style.Border.TopBorder = XLBorderStyleValues.Thick;
            // contents.LastRow().Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            ws.Columns().AdjustToContents(); // You can also specify the range of columns to adjust, e.g.
            // ws.Columns(2, 6).AdjustToContents(); or ws.Columns("2-6").AdjustToContents();

            wb.SaveAs(@"D:\Showcase.xlsx");
            */

            #endregion
        });
    }
}
