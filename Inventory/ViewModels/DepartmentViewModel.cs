namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Linq;

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
        });

        public ICommand EditDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();

            var editDepartmentWindow = new DepartmentEditWindow
            {
                DataContext = new DepartmentEditViewModel(SelectDepartment)
            };
            editDepartmentWindow.ShowDialog();
        }, () => SelectDepartment != null);

        public ICommand DeleteDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == SelectDepartment.Id_department);

            if (findDepartment == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            db.Departments.Remove(findDepartment);
            db.SaveChanges();

            foreach (var department in Departments.Where(department => department.Id_department == SelectDepartment.Id_department))
            {
                Departments.Remove(department);
                break;
            }
        }, () => SelectDepartment != null);
        #endregion
    }
}
