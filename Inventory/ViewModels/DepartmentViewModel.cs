namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Linq;

    using Inventory.View.Add;
    using Inventory.View.Edit;

    internal class DepartmentViewModel : BindableBase
    {
        #region Свойства
        public ObservableCollection<Department> Departments { get; set; }

        public Department SelectDepartment { get; set; }
        #endregion

        public DepartmentViewModel()
        {
            using var db = new InventoryEntities();
            Departments = new ObservableCollection<Department>(db.Departments.ToList());
        }

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
