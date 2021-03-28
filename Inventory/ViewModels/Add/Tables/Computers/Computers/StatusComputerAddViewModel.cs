﻿namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Computers;

    public class StatusComputerAddViewModel : BindableBase
    {
        public StatusComputerAddViewModel() => StatusComputer = new Statuses_computers();

        public Statuses_computers StatusComputer { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusComputer);
            StatusesComputersViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(StatusComputer.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
