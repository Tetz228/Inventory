namespace Inventory.ViewModels.Tables.Employees
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View.Add;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Add;
    using Inventory.ViewModels.Add.Tables.Employees;
    using Inventory.ViewModels.Edit;
    using Inventory.ViewModels.Edit.Tables.Employees;

    internal class DepartmentViewModel : BindableBase
    {
        public DepartmentViewModel()
        {
            using var db = new InventoryEntities();
            Departments = new ObservableCollection<Department>(db.Departments.ToList());
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
        }

        #region Свойства
        public ObservableCollection<Department> Departments { get; private set; }

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
        public ICommand DataGridMouseLeftButtonDown => new DelegateCommand(() => SelectDepartment = null);

        public ICommand AddDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();
            var addDepartmentWindow = new DepartmentAddWindow();
            var addDepartmentViewModel = new DepartmentAddViewModel();

            addDepartmentWindow.DataContext = addDepartmentViewModel;
            addDepartmentWindow.ShowDialog();

            Departments = new ObservableCollection<Department>(db.Departments.ToList());
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
        });

        public ICommand EditDepartment => new DelegateCommand<Department>((depart) =>
        {
            using var db = new InventoryEntities();
            var editDepartmentWindow = new DepartmentEditWindow();
            var editDepartmentViewModel = new DepartmentEditViewModel
            {
                Department = depart
            };

            editDepartmentWindow.DataContext = editDepartmentViewModel;
            editDepartmentWindow.ShowDialog();

            Departments = new ObservableCollection<Department>(db.Departments.ToList());
            DepartmentsCollection = CollectionViewSource.GetDefaultView(Departments);
        }, (depart) => depart != null);

        public ICommand DeleteDepartment => new DelegateCommand<Department>((depart) =>
        {
            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == depart.Id_department);

            if (findDepartment == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
                Departments = new ObservableCollection<Department>(db.Departments.ToList());
                return;
            }

            db.Departments.Remove(findDepartment);
            db.SaveChanges();

            Departments.Remove(depart);
        }, (depart) => depart != null);
        #endregion
    }
}