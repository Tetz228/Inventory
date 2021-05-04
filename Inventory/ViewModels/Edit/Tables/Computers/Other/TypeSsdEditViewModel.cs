namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;

    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class TypeSsdEditViewModel : BindableBase, IEditableObject
    {
        public TypeSsdEditViewModel(Types_ssd typeSsd)
        {
            TypeSsd = typeSsd;
            BeginEdit();
        }

        public Types_ssd TypeSsd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(TypeSsd.Id_type_ssd, TypeSsd);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(TypeSsd.ErrorCollection));

        #region Откат изменений
        private Types_ssd _selectTypeSsd;

        public void BeginEdit()
        {
            _selectTypeSsd = new Types_ssd
            {
                Id_type_ssd = TypeSsd.Id_type_ssd,
                Name = TypeSsd.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeSsd = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeSsd == null)
                return;

            TypeSsd.Id_type_ssd = _selectTypeSsd.Id_type_ssd;
            TypeSsd.Name = _selectTypeSsd.Name;
        }
        #endregion
    }
}
