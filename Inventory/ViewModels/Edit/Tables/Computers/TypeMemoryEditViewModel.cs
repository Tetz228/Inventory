namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

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
            Types_memory.EditTypeMemory(TypeMemory);
            editWindow.Close();
        }, _ => TypeMemory.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            TypeMemory.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
