namespace Inventory.ViewModels.Tables.Employees
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

    using Inventory.Services;

    public class DepartmentsViewModel : BindableBase
    {
        public DepartmentsViewModel()
        {
            using var db = new InventoryEntities();

            Departments = new ObservableCollection<Department>(db.Departments.AsNoTracking()).Sort(department => department.Name);
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
        }

        #region Свойства
        private ICollectionView DepartmentsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Department> Departments { get; set; }

        public Department SelectDepartment { get; set; }

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
                        return department.Search(DepartmentsFilter);

                    return false;
                };
                DepartmentsCollection.Refresh();
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
                            Departments.Sort(department => department.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectDepartment = null;
        #endregion

        #region Команды
        public ICommand AddDepartmentCommand => new DelegateCommand(() =>
        {
            var addDepartmentWindow = new DepartmentAddWindow();
            addDepartmentWindow.ShowDialog();
        });

        public ICommand EditDepartmentCommand => new DelegateCommand<Department>(department =>
        {
            var editDepartmentWindow = new DepartmentEditWindow();
            var editDepartmentViewModel = new DepartmentEditViewModel(department);
            editDepartmentWindow.DataContext = editDepartmentViewModel;
            editDepartmentWindow.Closing += editDepartmentViewModel.OnWindowClosing;
            editDepartmentWindow.ShowDialog();
        }, depart => depart != null);

        public ICommand DeleteDepartmentCommand => new DelegateCommand<Department>(selectDepartment =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectDepartment.Name}?", "Удаление отдела", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Department>(selectDepartment.Id_department);
            RefreshCollection();
        }, selectDepartment => selectDepartment != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Departments.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Departments.AsNoTracking())
                Departments.Add(item);
        }
    }
}