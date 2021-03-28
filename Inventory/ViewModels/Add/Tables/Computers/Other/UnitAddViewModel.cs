namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class UnitAddViewModel : BindableBase
    {
        public UnitAddViewModel()
        {
            Unit = new Unit();
        }

        public Unit Unit { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Unit);
            Unit.RefreshCollection();
            addWindow.Close();
        }, _ => Unit.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
