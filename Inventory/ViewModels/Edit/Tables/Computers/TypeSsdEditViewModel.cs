namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class TypeSsdEditViewModel : BindableBase
    {
        public TypeSsdEditViewModel(Types_ssd typeSsd)
        {
            TypeSsd = typeSsd;
            TypeSsd.BeginEdit();
        }

        public Types_ssd TypeSsd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => TypeSsd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeSsd.EndEdit();
            Types_ssd.EditTypeSsd(TypeSsd);
            editWindow.Close();
        }, _ => TypeSsd.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            TypeSsd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
