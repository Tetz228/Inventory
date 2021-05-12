namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class TypeHddAddViewModel : BindableBase
    {
        public Types_hdd TypeHdd { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            if (Services.Add(TypeHdd))
                TypesHddViewModel.TypesHdd.Add(TypeHdd);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypeHdd.ErrorCollection));
    }
}
