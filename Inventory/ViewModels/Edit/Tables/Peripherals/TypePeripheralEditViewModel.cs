namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class TypePeripheralEditViewModel : BindableBase, IEditableObject
    {
        public TypePeripheralEditViewModel(Types_peripherals typePeripheral)
        {
            TypePeripheral = typePeripheral;
            BeginEdit();
        }

        public Types_peripherals TypePeripheral { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(TypePeripheral.Id_type_peripheral, TypePeripheral);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(TypePeripheral.ErrorCollection));

        #region Откат изменений
        private Types_peripherals _selectTypePeripheral;

        public void BeginEdit()
        {
            _selectTypePeripheral = new Types_peripherals
            {
                Id_type_peripheral = TypePeripheral.Id_type_peripheral,
                Name = TypePeripheral.Name
            };
        }

        public void EndEdit()
        {
            _selectTypePeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectTypePeripheral == null)
                return;

            TypePeripheral.Id_type_peripheral = _selectTypePeripheral.Id_type_peripheral;
            TypePeripheral.Name = _selectTypePeripheral.Name;
        }
        #endregion
    }
}
