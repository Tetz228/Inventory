namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class TypePeripheralAddViewModel : BindableBase
    {
        public TypePeripheralAddViewModel()
        {
            TypePeripheral = new Types_peripherals();
        }

        public Types_peripherals TypePeripheral { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypePeripheral);
            Types_peripherals.RefreshCollection();
            addWindow.Close();
        }, _ => TypePeripheral.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
