namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

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
            Services.Add(OperatingSystem);
            Operating_systems.RefreshCollection();
            addWindow.Close();
        }, _ => OperatingSystem.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
