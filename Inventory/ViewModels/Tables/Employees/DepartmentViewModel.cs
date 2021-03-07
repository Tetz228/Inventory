namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;

    internal class DepartmentViewModel : BindableBase
    {
        public DepartmentViewModel()
        {
            using var db = new InventoryEntities();
            Departments = new ObservableCollection<Department>(db.Departments);
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
        }

        #region Свойства
        public static ObservableCollection<Department> Departments { get; set; }

        private ICollectionView DepartmentsCollection { get; set; }

        private string _searchDepartment;

        public string SearchDepartment
        {
            get => _searchDepartment;
            set
            {
                _searchDepartment = value;
                DepartmentsCollection.Filter = obj =>
                {
                    if (obj is Department department)
                        return department.Name.ToLower().Contains(SearchDepartment.ToLower());

                    return false;
                };
                DepartmentsCollection.Refresh();
            }
        }

        public Department SelectDepartment { get; set; }
        #endregion

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectDepartment = null);

        public ICommand AddDepartment => new DelegateCommand(() =>
        {
            var addDepartmentWindow = new DepartmentAddWindow();

            addDepartmentWindow.ShowDialog();
        });

        public ICommand EditDepartment => new DelegateCommand<Department>((depart) =>
        {
            var editDepartmentWindow = new DepartmentEditWindow();
            var editDepartmentViewModel = new DepartmentEditViewModel(depart);

            editDepartmentWindow.DataContext = editDepartmentViewModel;
            editDepartmentWindow.ShowDialog();

        }, (depart) => depart != null);

        public ICommand DeleteDepartment => new DelegateCommand<Department>(Delete, (selectDepart) => selectDepart != null);
        #endregion

        private async void Delete(Department department) => await Department.DeleteDepartment(department);
    }
}