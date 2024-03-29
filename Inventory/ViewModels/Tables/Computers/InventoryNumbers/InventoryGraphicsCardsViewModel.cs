﻿namespace Inventory.ViewModels.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.Base;

    public class InventoryGraphicsCardsViewModel : BaseViewModel<Inventory_numbers_graphics_cards>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации видеокарт.xlsx";
        private const string NAMED_AREA_NAME = "InventoryGraphicsCards";

        public InventoryGraphicsCardsViewModel() : base(InventoryGraphicsCards, RefreshCollection, NAME_TEMPLATE, NAMED_AREA_NAME) => RefreshCollection();

        public static ObservableCollection<Inventory_numbers_graphics_cards> InventoryGraphicsCards { get; set; } = new();
        
        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            InventoryGraphicsCards.Sort(numberHdd => numberHdd.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryGraphicsCards.Sort(manufacturer => manufacturer.Graphics_cards.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Memory_size, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddInventoryGraphicCardCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryGraphicCardAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryGraphicCardCommand => new DelegateCommand<Inventory_numbers_graphics_cards>(inventoryGraphicsCards =>
        {
            var editWindow = new InventoryGraphicCardEditWindow();
            var editViewModel = new InventoryGraphicCardEditViewModel(inventoryGraphicsCards);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryGraphicsCards => inventoryGraphicsCards != null);

        public ICommand DeleteInventoryGraphicCardCommand => new DelegateCommand<Inventory_numbers_graphics_cards>(selectInventoryGraphicCard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарную видеокарту:\nинвент. номер - {selectInventoryGraphicCard.Inventory_number};\nпроизводитель - {selectInventoryGraphicCard.Graphics_cards.Manufacturer.Name};\nнаименование - {selectInventoryGraphicCard.Graphics_cards.Name};\nобъём - {selectInventoryGraphicCard.Graphics_cards.Memory_size} {selectInventoryGraphicCard.Graphics_cards.Unit.Short_name}?", "Удаление инвентарной видеокарты", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_graphics_cards>(selectInventoryGraphicCard.Id_inventory_number_graphics_card))
                InventoryGraphicsCards.Remove(selectInventoryGraphicCard);
        }, selectInventoryGraphicCard => selectInventoryGraphicCard != null);

        public static void RefreshCollection()
        {
            InventoryGraphicsCards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_graphics_cards.AsNoTracking().Include(manufacturer => manufacturer.Graphics_cards.Manufacturer).Include(unit => unit.Graphics_cards.Unit))
                InventoryGraphicsCards.Add(item);

            InventoryGraphicsCards.Sort(manufacturer => manufacturer.Inventory_number);
        }
    }
}
