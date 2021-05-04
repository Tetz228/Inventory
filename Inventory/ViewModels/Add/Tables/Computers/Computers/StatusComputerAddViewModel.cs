﻿namespace Inventory.ViewModels.Add.Tables.Computers.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Computers;

    public class StatusComputerAddViewModel : BindableBase
    {
        public Statuses_computers StatusComputer { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusComputer);
            StatusesComputersViewModel.StatusesComputers.Add(StatusComputer);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(StatusComputer.ErrorCollection));
    }
}
