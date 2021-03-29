namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

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
            UnitsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Unit.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
