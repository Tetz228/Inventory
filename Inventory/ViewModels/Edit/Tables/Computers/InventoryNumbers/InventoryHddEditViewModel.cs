﻿namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;
    using Services;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryHddEditViewModel : BindableBase
    {
        public InventoryHddEditViewModel(Inventory_numbers_hdd inventoryHdd)
        {
            using var db = new InventoryEntities();

            Hdds = new ObservableCollection<Hdd>(db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_hdd).Include(unit => unit.Unit));
            InventoryHdd = inventoryHdd;
            InventoryHdd.InventoryNumberString = inventoryHdd.Inventory_number.ToString();
            InventoryHdd.BeginEdit();
        }

        public Inventory_numbers_hdd InventoryHdd { get; }

        public ObservableCollection<Hdd> Hdds { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryHdd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryHdd.EndEdit();
            Services.Edit(InventoryHdd.Id_inventory_number_hdd, InventoryHdd);
            InventoryHddViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryHdd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryHdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}