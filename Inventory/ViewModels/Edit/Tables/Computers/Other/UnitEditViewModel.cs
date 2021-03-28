namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class UnitEditViewModel : BindableBase
    {
        public UnitEditViewModel(Unit unit)
        {
            Unit = unit;
            Unit.BeginEdit();
        }

        public Unit Unit { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Unit.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Unit.EndEdit();
            Services.Edit(Unit.Id_unit, Unit);
            Unit.RefreshCollection();
            editWindow.Close();
        }, _ => Unit.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            Unit.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
