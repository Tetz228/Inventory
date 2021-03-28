namespace Inventory.ViewModels.Edit
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class ManufacturerEditViewModel : BindableBase
    {
        public ManufacturerEditViewModel(Manufacturer manufacturer)
        {
            Manufacturer = manufacturer;
            Manufacturer.BeginEdit();
        }

        public Manufacturer Manufacturer { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Manufacturer.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Manufacturer.EndEdit();
            Services.Edit(Manufacturer.Id_manufacturer, Manufacturer);
            Manufacturer.RefreshCollection();
            editWindow.Close();
        }, _ => Manufacturer.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            Manufacturer.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
