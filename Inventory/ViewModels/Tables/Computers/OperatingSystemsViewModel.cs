namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class OperatingSystemsViewModel : BindableBase
    {
        public OperatingSystemsViewModel()
        {
            using var db = new InventoryEntities();

            OperatingSystems = new ObservableCollection<Operating_systems>(db.Operating_systems);
            OperatingSystems.Sort(system => system.Name, SortDirection = ListSortDirection.Ascending);
            OperatingSystemsCollection = CollectionViewSource.GetDefaultView(OperatingSystems);
        }

        #region Свойства

        private ICollectionView OperatingSystemsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Operating_systems> OperatingSystems { get; set; }

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
                        return Operating_systems.SearchFor(operatingSystem, OperatingSystemsFilter);

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
                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                OperatingSystems.Sort(operatingSystem => operatingSystem.Name, SortDirection = ListSortDirection.Descending);
                            else
                                OperatingSystems.Sort(operatingSystem => operatingSystem.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Версия":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                OperatingSystems.Sort(operatingSystem => operatingSystem.System_version, SortDirection = ListSortDirection.Descending);
                            else
                                OperatingSystems.Sort(operatingSystem => operatingSystem.System_version, SortDirection = ListSortDirection.Ascending);
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

        public ICommand DeleteOperatingSystemCommand => new DelegateCommand<Operating_systems>(Operating_systems.DeleteOperatingSystem, selectOperatingSystem => selectOperatingSystem != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Operating_systems.RefreshCollection);
        #endregion
    }
}
