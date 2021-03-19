namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    internal class TypePeripheralAddViewModel : BindableBase
    {
        public TypePeripheralAddViewModel()
        {
            TypePeripheral = new Types_peripherals();
        }

        public Types_peripherals TypePeripheral { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Types_peripherals.AddTypePeripheral(TypePeripheral.Name);
            addWindow.Close();
        }, _ => TypePeripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
