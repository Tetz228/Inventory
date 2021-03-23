namespace Inventory.ViewModels.Add.Tables
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class ManufacturerAddViewModel : BindableBase
    {
        public ManufacturerAddViewModel()
        {
            Manufacturer = new Manufacturer();
        }

        public Manufacturer Manufacturer { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Manufacturer.AddManufacturer(Manufacturer.Name);
            addWindow.Close();
        }, _ => Manufacturer.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}