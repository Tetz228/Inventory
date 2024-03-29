﻿namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.Base;

    public class HddViewModel : BaseViewModel<Hdd>
    {
        public HddViewModel() : base(Hdd, RefreshCollection) => RefreshCollection();
        
        public static ObservableCollection<Hdd> Hdd { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Hdd.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Тип":
                        {
                            Hdd.Sort(type => type.Types_hdd.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Hdd.Sort(hdd => hdd.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            Hdd.Sort(hdd => hdd.Memory_size + " " + hdd.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddHddCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new HddAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditHddCommand => new DelegateCommand<Hdd>(hdd =>
        {
            var editWindow = new HddEditWindow();
            var editViewModel = new HddEditViewModel(hdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, hdd => hdd != null);

        public ICommand DeleteHddCommand => new DelegateCommand<Hdd>(selectHdd =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить жесткий диск:\nпроизводитель - {selectHdd.Manufacturer.Name};\nтип - {selectHdd.Types_hdd.Name};\nнаименование - {selectHdd.Name};\nобъём - {selectHdd.Memory_size} {selectHdd.Unit.Short_name}?", "Удаление жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Hdd>(selectHdd.Id_hdd))
                Hdd.Remove(selectHdd);
        }, selectHdd => selectHdd != null);

        public static void RefreshCollection()
        {
            Hdd.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Hdds.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_hdd))
            {
                Hdd.Add(item);
            }

            Hdd.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
