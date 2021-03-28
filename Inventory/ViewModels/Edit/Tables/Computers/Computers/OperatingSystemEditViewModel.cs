namespace Inventory.ViewModels.Edit.Tables.Computers.Computers
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class OperatingSystemEditViewModel : BindableBase
    {
        public OperatingSystemEditViewModel(Operating_systems operatingSystem)
        {
            OperatingSystem = operatingSystem;
            OperatingSystem.BeginEdit();
        }

        public Operating_systems OperatingSystem { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => OperatingSystem.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            OperatingSystem.EndEdit();
            Services.Edit(OperatingSystem.Id_operating_system, OperatingSystem);
            Operating_systems.RefreshCollection();
            editWindow.Close();
        }, _ => OperatingSystem.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            OperatingSystem.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
