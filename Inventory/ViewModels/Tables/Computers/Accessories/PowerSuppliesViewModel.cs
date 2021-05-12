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

    public class PowerSuppliesViewModel : BaseViewModel<Power_supplies>
    {
        public PowerSuppliesViewModel() : base(PowerSupplies, RefreshCollection) => RefreshCollection();
        
        public static ObservableCollection<Power_supplies> PowerSupplies { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            PowerSupplies.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            PowerSupplies.Sort(powerSupply => powerSupply.Name, SortDirection);
                            break;
                        }
                    case "Мощность":
                        {
                            PowerSupplies.Sort(powerSupply => powerSupply.Power + " " + powerSupply.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddPowerSupplyCommand => new DelegateCommand(() =>
        {
            var addWindow = new PowerSupplyAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditPowerSupplyCommand => new DelegateCommand<Power_supplies>(powerSupply =>
        {
            var editWindow = new PowerSupplyEditWindow();
            var editViewModel = new PowerSupplyEditViewModel(powerSupply);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, powerSupply => powerSupply != null);

        public ICommand DeletePowerSupplyCommand => new DelegateCommand<Power_supplies>(selectPowerSupply =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить блок питания:\nпроизводитель - {selectPowerSupply.Manufacturer.Name};\nнаименование - {selectPowerSupply.Name};\nмощность - {selectPowerSupply.Power} {selectPowerSupply.Unit.Short_name}?", "Удаление блока питания", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Power_supplies>(selectPowerSupply.Id_power_supplie))
                PowerSupplies.Remove(selectPowerSupply);
        }, selectPowerSupply => selectPowerSupply != null);

        public static void RefreshCollection()
        {
            PowerSupplies.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Power_supplies.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit))
                PowerSupplies.Add(item);

            PowerSupplies.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
