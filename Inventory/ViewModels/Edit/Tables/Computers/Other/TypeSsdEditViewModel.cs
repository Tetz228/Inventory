namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

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
            Services.Edit(TypeSsd.Id_type_ssd, TypeSsd);
            Types_ssd.RefreshCollection();
            editWindow.Close();
        }, _ => TypeSsd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeSsd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
