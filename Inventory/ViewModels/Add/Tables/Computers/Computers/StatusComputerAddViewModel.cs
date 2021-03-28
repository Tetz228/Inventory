namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class StatusComputerAddViewModel : BindableBase
    {
        public StatusComputerAddViewModel() => StatusComputer = new Statuses_computers();

        public Statuses_computers StatusComputer { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusComputer);
            Statuses_computers.RefreshCollection();
            addWindow.Close();
        }, _ => StatusComputer.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
