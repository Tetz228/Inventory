﻿namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class TypeMemoryEditViewModel : BindableBase, IEditableObject
    {
        public TypeMemoryEditViewModel(Types_memory typeMemory)
        {
            TypeMemory = typeMemory;
            BeginEdit();
        }

        public Types_memory TypeMemory { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(TypeMemory.Id_type_memory, TypeMemory);
            TypesMemoryViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => TypeMemory.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Types_memory _selectTypeMemory;

        public void BeginEdit()
        {
            _selectTypeMemory = new Types_memory
            {
                Id_type_memory = TypeMemory.Id_type_memory,
                Name = TypeMemory.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeMemory = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeMemory == null)
                return;

            TypeMemory.Id_type_memory = _selectTypeMemory.Id_type_memory;
            TypeMemory.Name = _selectTypeMemory.Name;
        }
        #endregion
    }
}
