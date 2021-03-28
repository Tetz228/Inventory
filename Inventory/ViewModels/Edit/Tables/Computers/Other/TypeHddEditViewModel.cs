namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class TypeHddEditViewModel : BindableBase, IEditableObject
    {
        public TypeHddEditViewModel(Types_hdd typeHdd)
        {
            TypeHdd = typeHdd;
            BeginEdit();
        }

        public Types_hdd TypeHdd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(TypeHdd.Id_type_hdd, TypeHdd);
            TypesHddsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => TypeHdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

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
