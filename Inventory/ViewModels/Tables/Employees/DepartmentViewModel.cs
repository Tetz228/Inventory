﻿namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    internal class DepartmentViewModel : BindableBase
    {
        public DepartmentViewModel()
        {
            using var db = new InventoryEntities();
            Departments = new ObservableCollection<Department>(db.Departments);
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
            DepartmentsCollection.SortDescriptions.Add(new SortDescription(nameof(Department.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Department> Departments { get; set; }

        public ICollectionView DepartmentsCollection { get; set; }

        private string _departmentsFilter;

        public string DepartmentsFilter
        {
            get => _departmentsFilter;
            set
            {
                _departmentsFilter = value;
                DepartmentsCollection.Filter = obj =>
                {
                    if (obj is Department department)
                        return Department.Search(department, DepartmentsFilter);

                    return false;
                };
                DepartmentsCollection.Refresh();
            }
        }

        public Department SelectDepartment { get; set; }
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
                    if (DepartmentsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                    {
                        DepartmentsCollection.SortDescriptions.Clear();
                        DepartmentsCollection.SortDescriptions.Add(new SortDescription(nameof(Department.Name), ListSortDirection.Descending));
                    }
                    else
                    {
                        DepartmentsCollection.SortDescriptions.Clear();
                        DepartmentsCollection.SortDescriptions.Add(new SortDescription(nameof(Department.Name), ListSortDirection.Ascending));
                    }

                    DepartmentsCollection.Refresh();

                    break;
                }
            }
        }

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectDepartment = null);

        public ICommand AddDepartment => new DelegateCommand(() =>
        {
            var addDepartmentWindow = new DepartmentAddWindow();

            addDepartmentWindow.ShowDialog();
        });

        public ICommand EditDepartment => new DelegateCommand<Department>(department =>
        {
            var editDepartmentWindow = new DepartmentEditWindow();
            var editDepartmentViewModel = new DepartmentEditViewModel(department);

            editDepartmentWindow.DataContext = editDepartmentViewModel;
            editDepartmentWindow.ShowDialog();

        }, depart => depart != null);

        public ICommand DeleteDepartment => new DelegateCommand<Department>(Department.DeleteDepartment, selectDepartment => selectDepartment != null);

        public ICommand RefreshList => new DelegateCommand(Department.RefreshCollection);
        #endregion
    }
}