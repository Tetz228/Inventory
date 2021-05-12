namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class UnitEditViewModel : BindableBase, IEditableObject
    {
        public UnitEditViewModel(Unit unit)
        {
            Unit = unit;
            BeginEdit();
        }

        public Unit Unit { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Unit.Id_unit, Unit);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Unit.ErrorCollection));

        #region Откат изменений
        private Unit _selectUnit;

        public void BeginEdit()
        {
            _selectUnit = new Unit
            {
                Id_unit = Unit.Id_unit,
                Full_name = Unit.Full_name,
                Short_name = Unit.Short_name
            };
        }

        public void EndEdit() => _selectUnit = null;

        public void CancelEdit()
        {
            if (_selectUnit == null)
                return;

            Unit.Id_unit = _selectUnit.Id_unit;
            Unit.Full_name = _selectUnit.Full_name;
            Unit.Short_name = _selectUnit.Short_name;
        }
        #endregion
    }
}
