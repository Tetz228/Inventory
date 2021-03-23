namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

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
            Types_hdd.EditTypeHdd(TypeHdd);
            editWindow.Close();
        }, _ => TypeHdd.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            TypeHdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
