namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    class TypePeripheralEditViewModel : BindableBase
    {
        public TypePeripheralEditViewModel(Types_peripherals typePeripheral)
        {
            TypePeripheral = typePeripheral;
            TypePeripheral.BeginEdit();
        }

        #region Свойства
        public Types_peripherals TypePeripheral { get; }
        #endregion

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypePeripheral.EndEdit();
            Types_peripherals.EditTypePeripheral(TypePeripheral);
            editWindow.Close();
        }, _ => TypePeripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            TypePeripheral.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
