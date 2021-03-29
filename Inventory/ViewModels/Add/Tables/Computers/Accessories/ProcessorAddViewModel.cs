using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Accessories;

    public class ProcessorAddViewModel : BindableBase
    {
        public ProcessorAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Units = new ObservableCollection<Unit>(db.Units);
            Sockets = new ObservableCollection<Socket>(db.Sockets);

            Processor = new Processor();
        }

        public Processor Processor { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public ObservableCollection<Socket> Sockets { get; }

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Processor);
            ProcessorsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Processor.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
