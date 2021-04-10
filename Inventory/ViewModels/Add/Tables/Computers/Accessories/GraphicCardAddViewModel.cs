﻿namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class GraphicCardAddViewModel : BindableBase
    {
        public GraphicCardAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
        }

        public Graphics_cards GraphicCard { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(GraphicCard);
            GraphicsCardsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(GraphicCard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
