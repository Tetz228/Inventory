﻿namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;
    using Services;
    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryMotherboardEditViewModel : BindableBase
    {
        public InventoryMotherboardEditViewModel(Inventory_numbers_motherboards inventoryMotherboard)
        {
            using var db = new InventoryEntities();

            Motherboards = new ObservableCollection<Motherboard>(db.Motherboards.AsNoTracking()
                    .Include(manufacturer => manufacturer.Manufacturer)
                    .Include(socket => socket.Socket))
                    .Sort(socket => socket.Socket.Name);
            InventoryMotherboard = inventoryMotherboard;
            InventoryMotherboard.BeginEdit();
        }

        public Inventory_numbers_motherboards InventoryMotherboard { get; }

        public ObservableCollection<Motherboard> Motherboards { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryMotherboard.CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryMotherboard.EndEdit();
            Services.Edit(InventoryMotherboard.Id_inventory_number_motherboard, InventoryMotherboard);
            InventoryMotherboardsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryMotherboard.ErrorCollection));
    }
}
