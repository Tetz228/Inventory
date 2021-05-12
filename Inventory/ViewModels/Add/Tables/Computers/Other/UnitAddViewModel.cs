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
        public Unit Unit { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            if(Services.Add(Unit)) 
                UnitsViewModel.Units.Add(Unit);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Unit.ErrorCollection));
    }
}
