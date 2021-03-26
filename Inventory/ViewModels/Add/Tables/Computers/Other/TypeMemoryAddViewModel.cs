﻿namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class TypeMemoryAddViewModel : BindableBase
    {
        public TypeMemoryAddViewModel()
        {
            TypeMemory = new Types_memory();
        }

        public Types_memory TypeMemory { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Types_memory.AddTypeMemory(TypeMemory.Name);
            addWindow.Close();
        }, _ => TypeMemory.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}