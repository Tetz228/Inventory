namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Computers;

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
            OperatingSystemsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => OperatingSystem.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
