namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class OperatingSystemAddViewModel : BindableBase
    {
        public OperatingSystemAddViewModel()
        {
            OperatingSystem = new Operating_systems();
        }

        public Operating_systems OperatingSystem { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Operating_systems.AddOperatingSystem(OperatingSystem.Name, OperatingSystem.System_version);
            addWindow.Close();
        }, _ => OperatingSystem.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
