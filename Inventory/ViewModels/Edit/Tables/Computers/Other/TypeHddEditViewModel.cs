namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class TypeHddEditViewModel : BindableBase, IEditableObject
    {
        public TypeHddEditViewModel(Types_hdd typeHdd)
        {
            TypeHdd = typeHdd;
            BeginEdit();
        }

        public Types_hdd TypeHdd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(TypeHdd.Id_type_hdd, TypeHdd);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(TypeHdd.ErrorCollection));

        #region Откат изменений
        private Types_hdd _selectTypeHdd;

        public void BeginEdit()
        {
            _selectTypeHdd = new Types_hdd
            {
                Id_type_hdd = TypeHdd.Id_type_hdd,
                Name = TypeHdd.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeHdd = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeHdd == null)
                return;

            TypeHdd.Id_type_hdd = _selectTypeHdd.Id_type_hdd;
            TypeHdd.Name = _selectTypeHdd.Name;
        }
        #endregion
    }
}
