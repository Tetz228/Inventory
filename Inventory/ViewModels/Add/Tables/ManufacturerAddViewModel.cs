namespace Inventory.ViewModels.Add.Tables
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables;

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
            Services.Add(Manufacturer);
            ManufacturersViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Manufacturer.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}