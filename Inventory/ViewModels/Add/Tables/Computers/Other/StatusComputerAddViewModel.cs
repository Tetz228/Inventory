namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class StatusComputerAddViewModel : BindableBase
    {
        public Statuses_computers StatusComputer { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            if (Services.Add(StatusComputer))
                StatusesComputersViewModel.StatusesComputers.Add(StatusComputer);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(StatusComputer.ErrorCollection));
    }
}
