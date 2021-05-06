namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class OperatingSystemAddViewModel : BindableBase
    {
        public Operating_systems OperatingSystem { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(OperatingSystem);
            OperatingSystemsViewModel.OperatingSystems.Add(OperatingSystem);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(OperatingSystem.ErrorCollection));
    }
}
