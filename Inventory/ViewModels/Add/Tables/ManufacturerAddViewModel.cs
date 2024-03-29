﻿namespace Inventory.ViewModels.Add.Tables
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables;
    using System.Windows;
    using System.Windows.Input;

    public class ManufacturerAddViewModel : BindableBase
    {
        public Manufacturer Manufacturer { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            if(Services.Add(Manufacturer))
                ManufacturersViewModel.Manufacturers.Add(Manufacturer);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Manufacturer.ErrorCollection));
    }
}