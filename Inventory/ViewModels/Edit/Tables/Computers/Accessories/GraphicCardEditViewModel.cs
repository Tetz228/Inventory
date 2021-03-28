﻿namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class GraphicCardEditViewModel : BindableBase
    {
        public GraphicCardEditViewModel(Graphics_cards graphicCard)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Units = new ObservableCollection<Unit>(db.Units);

            GraphicCard = graphicCard;
            GraphicCard.BeginEdit();
        }

        public Graphics_cards GraphicCard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => GraphicCard.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            GraphicCard.EndEdit();
            Graphics_cards.EditGraphicCard(GraphicCard);
            editWindow.Close();
        }, _ => GraphicCard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            GraphicCard.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}