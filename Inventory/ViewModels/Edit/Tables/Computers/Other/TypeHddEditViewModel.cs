namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class TypeHddEditViewModel : BindableBase
    {
        public TypeHddEditViewModel(Types_hdd typeHdd)
        {
            TypeHdd = typeHdd;
            TypeHdd.BeginEdit();
        }

        public Types_hdd TypeHdd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => TypeHdd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeHdd.EndEdit();
            Services.Edit(TypeHdd.Id_type_hdd, TypeHdd);
            Types_hdd.RefreshCollection();
            editWindow.Close();
        }, _ => TypeHdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            TypeHdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
