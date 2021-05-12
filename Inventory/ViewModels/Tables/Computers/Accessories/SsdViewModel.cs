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

    public class SsdViewModel : BaseViewModel<Ssd>
    {
        public SsdViewModel() : base(Ssd, RefreshCollection) => RefreshCollection();
        
        public static ObservableCollection<Ssd> Ssd { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Ssd.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            Ssd.Sort(type => type.Types_ssd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Ssd.Sort(ssd => ssd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Ssd.Sort(ssd => ssd.Memory_size + " " + ssd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }
        
        public ICommand AddSsdCommand => new DelegateCommand(() =>
        {
            var addSsdWindow = new SsdAddWindow();
            addSsdWindow.ShowDialog();
        });

        public ICommand EditSsdCommand => new DelegateCommand<Ssd>(ssd =>
        {
            var editWindow = new SsdEditWindow();
            var editViewModel = new SsdEditViewModel(ssd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, ssd => ssd != null);

        public ICommand DeleteSsdCommand => new DelegateCommand<Ssd>(selectSsd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить SSD-накопитель:\nпроизводитель - {selectSsd.Manufacturer.Name};\nтип - {selectSsd.Types_ssd.Name};\nнаименование - {selectSsd.Name};\nОбъём - {selectSsd.Memory_size} {selectSsd.Unit.Short_name}?", "Удаление SSD-накопителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Ssd>(selectSsd.Id_ssd))
                Ssd.Remove(selectSsd);
        }, selectSsd => selectSsd != null);

        public static void RefreshCollection()
        {
            Ssd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Ssds.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_ssd))
                Ssd.Add(item);

            Ssd.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
