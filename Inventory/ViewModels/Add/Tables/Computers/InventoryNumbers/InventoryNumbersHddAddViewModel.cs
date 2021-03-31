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

    public class InventoryNumbersHddAddViewModel : BindableBase
    {
        public InventoryNumbersHddAddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumberHdd = new Inventory_numbers_hdd();
            Hdds = new ObservableCollection<Hdd>(db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_hdd).Include(unit => unit.Unit));
            InventoryNumberHdd.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_hdd InventoryNumberHdd { get; }

        public ObservableCollection<Hdd> Hdds { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_hdd.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_hdd.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryNumberHdd);
            InventoryNumbersHddViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryNumberHdd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
