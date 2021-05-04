namespace Inventory.ViewModels.Edit.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;

    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class OperatingSystemEditViewModel : BindableBase, IEditableObject
    {
        public OperatingSystemEditViewModel(Operating_systems operatingSystem)
        {
            OperatingSystem = operatingSystem;
            BeginEdit();
        }

        public Operating_systems OperatingSystem { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(OperatingSystem.Id_operating_system, OperatingSystem);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(OperatingSystem.ErrorCollection));

        #region Откат изменений
        private Operating_systems _selectOperatingSystem;

        public void BeginEdit()
        {
            _selectOperatingSystem = new Operating_systems
            {
                Id_operating_system = OperatingSystem.Id_operating_system,
                Name = OperatingSystem.Name,
                System_version = OperatingSystem.System_version
            };
        }

        public void EndEdit()
        {
            _selectOperatingSystem = null;
        }

        public void CancelEdit()
        {
            if (_selectOperatingSystem == null)
                return;

            OperatingSystem.Id_operating_system = _selectOperatingSystem.Id_operating_system;
            OperatingSystem.Name = _selectOperatingSystem.Name;
            OperatingSystem.System_version = _selectOperatingSystem.System_version;
        }
        #endregion
    }
}
