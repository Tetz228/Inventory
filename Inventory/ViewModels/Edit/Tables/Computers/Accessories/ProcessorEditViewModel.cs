namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class ProcessorEditViewModel : BindableBase, IEditableObject
    {
        public ProcessorEditViewModel(Processor processor)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
            Sockets = new ObservableCollection<Socket>(db.Sockets.AsNoTracking());

            Processor = processor;
            Processor.BaseFrequencyString = processor.Base_frequency.ToString(CultureInfo.InvariantCulture);
            Processor.AmountCoresString = processor.Amount_cores.ToString();
            BeginEdit();
        }

        public Processor Processor { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public ObservableCollection<Socket> Sockets { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Processor.Id_processor, Processor);
            ProcessorsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Processor.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Processor _selectProcessor;

        public void BeginEdit()
        {
            _selectProcessor = new Processor()
            {
                Id_processor = Processor.Id_processor,
                Base_frequency = Processor.Base_frequency,
                Amount_cores = Processor.Amount_cores,
                Fk_socket = Processor.Fk_socket,
                Name = Processor.Name,
                Fk_unit = Processor.Fk_unit,
                Fk_manufacturer = Processor.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectProcessor = null;
        }

        public void CancelEdit()
        {
            if (_selectProcessor == null)
                return;

            Processor.Id_processor = _selectProcessor.Id_processor;
            Processor.Base_frequency = _selectProcessor.Base_frequency;
            Processor.Amount_cores = _selectProcessor.Amount_cores;
            Processor.Fk_socket = _selectProcessor.Fk_socket;
            Processor.Name = _selectProcessor.Name;
            Processor.Fk_unit = _selectProcessor.Fk_unit;
            Processor.Fk_manufacturer = _selectProcessor.Fk_manufacturer;
        }
        #endregion
    }
}
