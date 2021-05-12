namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.Base;

    public class MotherboardsViewModel : BaseViewModel<Motherboard>
    {
        public MotherboardsViewModel() : base(Motherboards, RefreshCollection) => RefreshCollection();
        
        public static ObservableCollection<Motherboard> Motherboards { get; set; } = new();
       
        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Motherboards.Sort(graphicCard => graphicCard.Name, SortDirection);
                            break;
                        }
                    case "Сокет":
                        {
                            Motherboards.Sort(socket => socket.Socket.Name, SortDirection);
                            break;
                        }
                }
            }
        }
        
        public ICommand AddMotherboardCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new MotherboardAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditMotherboardCommand => new DelegateCommand<Motherboard>(motherboard =>
        {
            var editWindow = new MotherboardEditWindow();
            var editViewModel = new MotherboardEditViewModel(motherboard);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, motherboard => motherboard != null);

        public ICommand DeleteMotherboardCommand => new DelegateCommand<Motherboard>(selectMotherboard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить мат. плату:\nпроизводитель - {selectMotherboard.Manufacturer.Name};\nнаименование - {selectMotherboard.Name};\nсокет - {selectMotherboard.Socket.Name}?", "Удаление материнской платы", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Motherboard>(selectMotherboard.Id_motherboard))
                Motherboards.Remove(selectMotherboard);
        }, selectMotherboard => selectMotherboard != null);

        public static void RefreshCollection()
        {
            Motherboards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Motherboards.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(socket => socket.Socket))
                Motherboards.Add(item);

            Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
