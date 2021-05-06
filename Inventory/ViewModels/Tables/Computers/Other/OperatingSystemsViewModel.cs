﻿namespace Inventory.ViewModels.Tables.Computers.Other
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Other;
    using Inventory.View.Edit.Tables.Computers.Other;
    using Inventory.ViewModels.Edit.Tables.Computers.Other;

    public class OperatingSystemsViewModel : BindableBase
    {
        public OperatingSystemsViewModel()
        {
            RefreshCollection();
            OperatingSystemsCollection = CollectionViewSource.GetDefaultView(OperatingSystems);
        }

        #region Свойства

        private ICollectionView OperatingSystemsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Operating_systems> OperatingSystems { get; set; } = new();

        public Operating_systems SelectOperatingSystem { get; set; }

        private string _operatingSystemsFilter = string.Empty;

        public string OperatingSystemsFilter
        {
            get => _operatingSystemsFilter;
            set
            {
                _operatingSystemsFilter = value;
                OperatingSystemsCollection.Filter = obj =>
                {
                    if (obj is Operating_systems operatingSystem)
                        return operatingSystem.Search(OperatingSystemsFilter);

                    return false;
                };
                OperatingSystemsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            OperatingSystems.Sort(operatingSystem => operatingSystem.Name, SortDirection);
                            break;
                        }
                    case "Версия":
                        {
                            OperatingSystems.Sort(operatingSystem => operatingSystem.System_version, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectOperatingSystem = null;
        #endregion

        #region Команды
        public ICommand AddOperatingSystemCommand => new DelegateCommand(() =>
        {
            var addWindow = new OperatingSystemAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditOperatingSystemCommand => new DelegateCommand<Operating_systems>(operatingSystem =>
        {
            var editWindow = new OperatingSystemEditWindow();
            var editViewModel = new OperatingSystemEditViewModel(operatingSystem);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, operatingSystems => operatingSystems != null);

        public ICommand DeleteOperatingSystemCommand => new DelegateCommand<Operating_systems>(selectOperatingSystem =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить операционную систему:\nнаименование - {selectOperatingSystem.Name};\nверсия - {selectOperatingSystem.System_version}?", "Удаление операционной системы", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Operating_systems>(selectOperatingSystem.Id_operating_system);
            OperatingSystems.Remove(selectOperatingSystem);
        }, selectOperatingSystem => selectOperatingSystem != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        private static void RefreshCollection()
        {
            OperatingSystems.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Operating_systems.AsNoTracking())
                OperatingSystems.Add(item);

            OperatingSystems.Sort(system => system.Name);
        }
    }
}