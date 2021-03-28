﻿namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class TypeSddAddViewModel : BindableBase
    {
        public TypeSddAddViewModel()
        {
            TypeSsd = new Types_ssd();
        }

        public Types_ssd TypeSsd { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypeSsd);
            TypesSddsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => TypeSsd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
