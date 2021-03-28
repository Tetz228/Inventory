namespace Inventory.ViewModels.Edit
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables;

    public class ManufacturerEditViewModel : BindableBase, IEditableObject
    {
        public ManufacturerEditViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            BeginEdit();
        }

        public Manufacturer Manufacturer { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Manufacturer.Id_manufacturer, Manufacturer);
            ManufacturersViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Manufacturer.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

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
