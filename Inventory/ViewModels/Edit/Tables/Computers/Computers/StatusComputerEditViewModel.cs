namespace Inventory.ViewModels.Edit.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class StatusComputerEditViewModel : BindableBase
    {
        public StatusComputerEditViewModel(Statuses_computers statusComputer)
        {
            StatusComputer = statusComputer;
            StatusComputer.BeginEdit();
        }

        public Statuses_computers StatusComputer { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => StatusComputer.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            StatusComputer.EndEdit();
            Services.Edit(StatusComputer.Id_status_computer, StatusComputer);
            Statuses_computers.RefreshCollection();
            editWindow.Close();
        }, _ => StatusComputer.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            StatusComputer.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
