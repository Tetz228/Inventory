namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class TypeSddAddViewModel : BindableBase
    {
        public Types_ssd TypeSsd { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypeSsd);
            TypesSddsViewModel.TypesSsds.Add(TypeSsd);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypeSsd.ErrorCollection));
    }
}
