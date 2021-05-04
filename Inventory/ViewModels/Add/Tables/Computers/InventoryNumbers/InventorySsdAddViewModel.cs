﻿namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class InventorySsdAddViewModel : BindableBase
    {
        public InventorySsdAddViewModel()
        {
            using var db = new InventoryEntities();

            Ssds = new ObservableCollection<Ssd>(db.Ssds.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(type => type.Types_ssd)
                .Include(unit => unit.Unit))
                .Sort(manufact => manufact.Name);

            try
            {
                InventorySsd.Inventory_number = db.Inventory_numbers_ssd.Select(ssd => ssd.Inventory_number).Max() + 1;
            }
            catch
            {
                InventorySsd.Inventory_number = 1;
            }
        }

        public Inventory_numbers_ssd InventorySsd { get; } = new();

        public ObservableCollection<Ssd> Ssds { get; }
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventorySsd);
            InventorySsdViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventorySsd.ErrorCollection));
    }
}
