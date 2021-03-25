namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

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
            Operating_systems.EditOperatingSystem(OperatingSystem);
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
