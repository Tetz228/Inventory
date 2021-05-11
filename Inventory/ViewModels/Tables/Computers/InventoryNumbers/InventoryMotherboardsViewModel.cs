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

    public class InventoryMotherboardsViewModel : BaseViewModel<Inventory_numbers_motherboards>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации материнских плат.xlsx";
        private const string NAMED_AREA_NAME = "InventoryMotherboard";

        public InventoryMotherboardsViewModel() : base(InventoryMotherboards) => RefreshCollection();

        #region Свойства

        public static ObservableCollection<Inventory_numbers_motherboards> InventoryMotherboards { get; set; } = new();

        private string _inventoryMotherboardsFilter = string.Empty;

        public string InventoryMotherboardsFilter
        {
            get => _inventoryMotherboardsFilter;
            set
            {
                _inventoryMotherboardsFilter = value;
                CollectionView.Filter = obj =>
                {
                    if (obj is Inventory_numbers_motherboards inventoryMotherboard)
                        return inventoryMotherboard.Search(InventoryMotherboardsFilter);

                    return false;
                };
                CollectionView.Refresh();
            }
        }

        #endregion

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            InventoryMotherboards.Sort(motherboard => motherboard.Inventory_number, SortDirection);
                            break;
                        }
                    case "Производитель":
                        {
                            InventoryMotherboards.Sort(manufacturer => manufacturer.Motherboard.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            InventoryMotherboards.Sort(motherboard => motherboard.Motherboard.Name, SortDirection);
                            break;
                        }
                    case "Сокет":
                        {
                            InventoryMotherboards.Sort(socket => socket.Motherboard.Socket.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        #region Команды

        public ICommand AddInventoryMotherboardCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryMotherboardAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryMotherboardCommand => new DelegateCommand<Inventory_numbers_motherboards>(inventoryMotherboard =>
        {
            var editWindow = new InventoryMotherboardEditWindow();
            var editViewModel = new InventoryMotherboardEditViewModel(inventoryMotherboard);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryMotherboard => inventoryMotherboard != null);

        public ICommand DeleteInventoryMotherboardCommand => new DelegateCommand<Inventory_numbers_motherboards>(selectInventoryMotherboard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарную мат. плату:\nинвент. номер - {selectInventoryMotherboard.Inventory_number};\nпроизводитель - {selectInventoryMotherboard.Motherboard.Manufacturer.Name};\nнаименование - {selectInventoryMotherboard.Motherboard.Name};\nсокет - {selectInventoryMotherboard.Motherboard.Socket.Name}?",
                "Удаление инвентарной материнской платы", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_motherboards>(selectInventoryMotherboard.Id_inventory_number_motherboard))
                InventoryMotherboards.Remove(selectInventoryMotherboard);
        }, selectInventoryMotherboard => selectInventoryMotherboard != null);

        public ICommand ExportExcelCommand => new DelegateCommand<ICollectionView>(collectionView => collectionView.ExportExcel(NAME_TEMPLATE, NAMED_AREA_NAME));

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        #endregion

        public static void RefreshCollection()
        {
            InventoryMotherboards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_motherboards.AsNoTracking()
                .Include(manufacturer => manufacturer.Motherboard.Manufacturer)
                .Include(socket => socket.Motherboard.Socket))
            {
                InventoryMotherboards.Add(item);
            }

            InventoryMotherboards.Sort(inventoryMotherboards => inventoryMotherboards.Inventory_number);
        }
    }
}