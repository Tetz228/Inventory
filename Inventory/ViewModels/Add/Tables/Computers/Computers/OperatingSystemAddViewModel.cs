namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using System.Windows;
    using System.Windows.Input;

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
