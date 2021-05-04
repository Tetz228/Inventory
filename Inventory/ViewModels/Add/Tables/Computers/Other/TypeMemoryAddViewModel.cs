namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Windows;
    using System.Windows.Input;

    public class TypeMemoryAddViewModel : BindableBase
    {
        public Types_memory TypeMemory { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypeMemory);
            TypesMemoryViewModel.TypesMemory.Add(TypeMemory);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypeMemory.ErrorCollection));
    }
}
