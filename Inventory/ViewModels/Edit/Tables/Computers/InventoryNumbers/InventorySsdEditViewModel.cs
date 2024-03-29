﻿namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    public class InventorySsdEditViewModel : BindableBase
    {
        public InventorySsdEditViewModel(Inventory_numbers_ssd inventorySsd)
        {
            using var db = new InventoryEntities();

            Ssd = new ObservableCollection<Ssd>(db.Ssds.AsNoTracking()
                    .Include(manufacturer => manufacturer.Manufacturer)
                    .Include(type => type.Types_ssd)
                    .Include(unit => unit.Unit))
                .Sort(manufact => manufact.Name);
            InventorySsd = inventorySsd;
            InventorySsd.BeginEdit();
        }

        public Inventory_numbers_ssd InventorySsd { get; }

        public ObservableCollection<Ssd> Ssd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventorySsd.CancelEdit();
        
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventorySsd.EndEdit();
            Services.Edit(InventorySsd.Id_inventory_number_ssd, InventorySsd);
            InventorySsdViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventorySsd.ErrorCollection));
    }
}