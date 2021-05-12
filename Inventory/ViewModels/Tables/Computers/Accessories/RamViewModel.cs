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

    public class RamViewModel : BaseViewModel<Ram>
    {
        public RamViewModel() : base(Ram, RefreshCollection) => RefreshCollection();

        public static ObservableCollection<Ram> Ram { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Ram.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Ram.Sort(ram => ram.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Ram.Sort(memory => memory.Memory_size + " " + memory.Unit.Short_name, SortDirection);
                            break;
                        }
                    case "Тактовая частота":
                        {
                            Ram.Sort(clock => clock.Clock_frequency, SortDirection);
                            break;
                        }
                    case "Тип памяти":
                        {
                            Ram.Sort(type => type.Types_memory.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddRamCommand => new DelegateCommand(() =>
        {
            var addWindow = new RamAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditRamCommand => new DelegateCommand<Ram>(ram =>
        {
            var editWindow = new RamEditWindow();
            var editViewModel = new RamEditViewModel(ram);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, ram => ram != null);

        public ICommand DeleteRamCommand => new DelegateCommand<Ram>(selectRam =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить оперативную память:\nпроизводитель - {selectRam.Manufacturer.Name};\nнаименование - {selectRam.Name};\nтип памяти - {selectRam.Types_memory.Name};\nобъём - {selectRam.Memory_size} {selectRam.Unit.Short_name};\nтактовая частота - {selectRam.Clock_frequency}?", "Удаление оперативной памяти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Ram>(selectRam.Id_ram))
                Ram.Remove(selectRam);
        }, selectRam => selectRam != null);

        public static void RefreshCollection()
        {
            Ram.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Rams.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_memory))
                Ram.Add(item);

            Ram.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
