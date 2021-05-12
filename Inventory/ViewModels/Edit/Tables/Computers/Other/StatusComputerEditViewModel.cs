namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;

    public class StatusComputerEditViewModel : BindableBase, IEditableObject
    {
        public StatusComputerEditViewModel(Statuses_computers statusComputer)
        {
            StatusComputer = statusComputer;
            BeginEdit();
        }

        public Statuses_computers StatusComputer { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();
        
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(StatusComputer.Id_status_computer, StatusComputer);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(StatusComputer.ErrorCollection));

        #region Откат изменений
        private Statuses_computers _selectStatusComputer;

        public void BeginEdit()
        {
            _selectStatusComputer = new Statuses_computers
            {
                Id_status_computer = StatusComputer.Id_status_computer,
                Name = StatusComputer.Name
            };
        }

        public void EndEdit() => _selectStatusComputer = null;

        public void CancelEdit()
        {
            if (_selectStatusComputer == null)
                return;

            StatusComputer.Id_status_computer = _selectStatusComputer.Id_status_computer;
            StatusComputer.Name = _selectStatusComputer.Name;
        }
        #endregion
    }
}
