namespace TestConsole
{
    using System;
    using System.Collections.Generic;

    using ClosedXML.Excel;
    using System.Data;

    class Program
    {
        static void Main(string[] args)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Список сокетов");

            var listOfArr = new List<Int32[]>();
            listOfArr.Add(new Int32[] { 1, 2, 3 });
            listOfArr.Add(new Int32[] { 1 });
            listOfArr.Add(new Int32[] { 1, 2, 3, 4, 5, 6 });
            ws.Cell(1, 3).Value = "Arrays";
            ws.Range(1, 3, 1, 8).Merge().AddToNamed("Titles");
            ws.Cell(2, 3).Value = listOfArr;

            //DataTable dataTable = new DataTable();
            //dataTable.Columns.Add("Наименование", typeof(string));
            //dataTable.Columns.Add("Тип", typeof(string));
            //dataTable.Rows.Add("AM3", "AMD");
            //dataTable.Rows.Add("AM4", "INTEL");
            //ws.Cell(1, 1).Value = "Сокеты";
            ////Название таблицы
            //var tableName = ws.Range(1, 1, 1, 2).Merge().AddToNamed("TableName");
            ////Названия столбцов
            //var titles = ws.Range(2, 1, 2, 2).AddToNamed("Titles");
            ////Вставка данных в ячейки и задание стиля для них
            //var table = ws.Cell(2, 1).InsertTable(dataTable.AsEnumerable());
            //table.Theme = XLTableTheme.None;
            //table.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            //table.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            //table.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            //table.Style.Border.TopBorder = XLBorderStyleValues.Medium;

            //// Задание стиля для названия таблицы
            //var tableNameStyle = wb.Style;
            //tableNameStyle.Font.Bold = true;
            //tableNameStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //tableNameStyle.Fill.BackgroundColor = XLColor.White;

            //// Задание стиля для названия столбцов
            //var titlesStyle = wb.Style;
            //titlesStyle.Font.Bold = true;
            //titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //titlesStyle.Fill.BackgroundColor = XLColor.White;

            //// Format all titles in one shot
            //wb.NamedRanges.NamedRange("TableName").Ranges.Style = tableNameStyle;
            //wb.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;

            //tableName.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            //tableName.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            //tableName.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            //tableName.Style.Border.TopBorder = XLBorderStyleValues.Medium;

            //titles.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            //titles.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            //titles.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            //titles.Style.Border.TopBorder = XLBorderStyleValues.Medium;

            //ws.Columns().AdjustToContents();
            wb.SaveAs(@"D:\InsertingTables1.xlsx");
        }
    }
}
