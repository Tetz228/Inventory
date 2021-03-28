namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class TypeMemoryEditViewModel : BindableBase
    {
        public TypeMemoryEditViewModel(Types_memory typeMemory)
        {
            TypeMemory = typeMemory;
            TypeMemory.BeginEdit();
        }

        public Types_memory TypeMemory { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => TypeMemory.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeMemory.EndEdit();
            Services.Edit(TypeMemory.Id_type_memory, TypeMemory);
            Types_memory.RefreshCollection();
            editWindow.Close();
        }, _ => TypeMemory.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeMemory.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
