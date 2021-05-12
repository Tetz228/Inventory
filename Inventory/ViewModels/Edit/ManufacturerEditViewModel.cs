namespace Inventory.ViewModels.Edit
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class ManufacturerEditViewModel : BindableBase, IEditableObject
    {
        public ManufacturerEditViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            BeginEdit();
        }

        public Manufacturer Manufacturer { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Manufacturer.Id_manufacturer, Manufacturer);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Manufacturer.ErrorCollection));

        #region Откат изменений
        private Manufacturer _selectManufacturer;

        public void BeginEdit()
        {
            _selectManufacturer = new Manufacturer
            {
                Id_manufacturer = Manufacturer.Id_manufacturer,
                Name = Manufacturer.Name
            };
        }

        public void EndEdit()
        {
            _selectManufacturer = null;
        }

        public void CancelEdit()
        {
            if (_selectManufacturer == null)
                return;

            Manufacturer.Id_manufacturer = _selectManufacturer.Id_manufacturer;
            Manufacturer.Name = _selectManufacturer.Name;
        }
        #endregion
    }
}
