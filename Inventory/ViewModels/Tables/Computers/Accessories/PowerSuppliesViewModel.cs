using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;

    public class PowerSuppliesViewModel : BindableBase
    {
        public PowerSuppliesViewModel()
        {
            using var db = new InventoryEntities();

            PowerSupplies = new ObservableCollection<Power_supplies>(db.Power_supplies.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit));
            PowerSupplies.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            PowerSuppliesCollection = CollectionViewSource.GetDefaultView(PowerSupplies);
        }

        #region Свойства
        private ICollectionView PowerSuppliesCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Power_supplies SelectPowerSupply { get; set; }

        public static ObservableCollection<Power_supplies> PowerSupplies { get; set; }

        private string _powerSuppliesFilter = string.Empty;

        public string PowerSuppliesFilter
        {
            get => _powerSuppliesFilter;
            set
            {
                _powerSuppliesFilter = value;
                PowerSuppliesCollection.Filter = obj =>
                {
                    if (obj is Power_supplies powerSupply)
                        return powerSupply.Search(PowerSuppliesFilter);

                    return false;
                };
                PowerSuppliesCollection.Refresh();
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
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                PowerSupplies.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                PowerSupplies.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                PowerSupplies.Sort(powerSupply => powerSupply.Name, SortDirection = ListSortDirection.Descending);
                            else
                                PowerSupplies.Sort(powerSupply => powerSupply.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Объём":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                PowerSupplies.Sort(powerSupply => powerSupply.Power, SortDirection = ListSortDirection.Descending);
                            else
                                PowerSupplies.Sort(powerSupply => powerSupply.Power, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Единица измерения":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                PowerSupplies.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                            else
                                PowerSupplies.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectPowerSupply = null;
        #endregion

        #region Команды
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
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectPowerSupply.Manufacturer.Name} {selectPowerSupply.Name} {selectPowerSupply.Power} {selectPowerSupply.Unit.Short_name}?", "Удаление блока питания", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Power_supplies>(selectPowerSupply.Id_power_supplie);
            RefreshCollection();
        }, selectPowerSupply => selectPowerSupply != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            PowerSupplies.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Power_supplies.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit))
                PowerSupplies.Add(item);
        }
    }
}
