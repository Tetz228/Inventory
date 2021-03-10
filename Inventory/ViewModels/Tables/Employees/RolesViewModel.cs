using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Tables.Employees
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;

    class RolesViewModel : BindableBase
    {

        public RolesViewModel()
        {
            using var db = new InventoryEntities();
            Roles = new ObservableCollection<Role>(db.Roles);
            RolesCollection = CollectionViewSource.GetDefaultView(Roles);
            RolesCollection.SortDescriptions.Add(new SortDescription(nameof(Role.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Role> Roles { get; set; }

        public ICollectionView RolesCollection { get; set; }

        private string _rolesFilter;

        public string RolesFilter
        {
            get => _rolesFilter;
            set
            {
                _rolesFilter = value;
                RolesCollection.Filter = obj =>
                {
                    if (obj is Role role)
                        return Role.Search(role, RolesFilter);

                    return false;
                };
                RolesCollection.Refresh();
            }
        }

        public Role SelectRole { get; set; }
        #endregion

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                {
                    if (RolesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                    {
                        RolesCollection.SortDescriptions.Clear();
                        RolesCollection.SortDescriptions.Add(new SortDescription(nameof(Role.Name), ListSortDirection.Descending));
                    }
                    else
                    {
                        RolesCollection.SortDescriptions.Clear();
                        RolesCollection.SortDescriptions.Add(new SortDescription(nameof(Role.Name), ListSortDirection.Ascending));
                    }

                    RolesCollection.Refresh();

                    break;
                }
            }
        }

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectRole = null);

        public ICommand AddRole => new DelegateCommand(() =>
        {
            //var addDepartmentWindow = new DepartmentAddWindow();

            //addDepartmentWindow.ShowDialog();
        });

        public ICommand EditRole => new DelegateCommand<Role>(role =>
        {
            //var editDepartmentWindow = new DepartmentEditWindow();
            //var editDepartmentViewModel = new DepartmentEditViewModel(depart);

            //editDepartmentWindow.DataContext = editDepartmentViewModel;
            //editDepartmentWindow.ShowDialog();

        }, (role) => role != null);

        public ICommand DeleteRole => new DelegateCommand<Role>(Role.DeleteRole, selectRole => selectRole != null);

        public ICommand RefreshList => new DelegateCommand(Role.RefreshCollection);
        #endregion
    }
}
