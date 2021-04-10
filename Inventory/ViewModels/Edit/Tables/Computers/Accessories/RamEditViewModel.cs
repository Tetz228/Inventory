namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;

    public class RamEditViewModel : BindableBase, IEditableObject
    {
        public RamEditViewModel(Ram ram)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
            TypesMemories = new ObservableCollection<Types_memory>(db.Types_memory.AsNoTracking());

            Ram = ram;
            Ram.ClockFrequencyString = ram.Clock_frequency.ToString(CultureInfo.InvariantCulture);
            Ram.MemorySizeString = ram.Memory_size.ToString(CultureInfo.InvariantCulture);

            BeginEdit();
        }

        public Ram Ram { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public ObservableCollection<Types_memory> TypesMemories { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Ram.Id_ram, Ram);
            RamViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Ram.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Ram _selectRam;

        public void BeginEdit()
        {
            _selectRam = new Ram()
            {
                Id_ram = Ram.Id_ram,
                Clock_frequency = Ram.Clock_frequency,
                Memory_size = Ram.Memory_size,
                Fk_type_memory = Ram.Fk_type_memory,
                Name = Ram.Name,
                Fk_unit = Ram.Fk_unit,
                Fk_manufacturer = Ram.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectRam = null;
        }

        public void CancelEdit()
        {
            if (_selectRam == null)
                return;

            Ram.Id_ram = _selectRam.Id_ram;
            Ram.Clock_frequency = _selectRam.Clock_frequency;
            Ram.Memory_size = _selectRam.Memory_size;
            Ram.Fk_type_memory = _selectRam.Fk_type_memory;
            Ram.Name = _selectRam.Name;
            Ram.Fk_unit = _selectRam.Fk_unit;
            Ram.Fk_manufacturer = _selectRam.Fk_manufacturer;
        }
        #endregion
    }
}
