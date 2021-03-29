namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class TypeMemoryAddViewModel : BindableBase
    {
        public TypeMemoryAddViewModel()
        {
            TypeMemory = new Types_memory();
        }

        public Types_memory TypeMemory { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypeMemory);
            TypesMemoryViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypeMemory.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
