namespace Inventory.ViewModels.Edit
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class ManufacturerEditViewModel : BindableBase
    {
        public ManufacturerEditViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            Manufacturer.BeginEdit();
        }

        #region Свойства
        public Manufacturer Manufacturer { get; }
        #endregion

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Manufacturer.EndEdit();
            Manufacturer.EditManufacturer(Manufacturer);
            editWindow.Close();
        }, _ => Manufacturer.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Manufacturer.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
