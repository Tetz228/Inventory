namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class DepartmentEditViewModel : BindableBase
    {
        public Department Department { get; set; }

        public string NameDepartment { get; set; }

        public Action CloseWindow { get; set; }

        public DepartmentEditViewModel(Department department, Action closeWindow)
        {
            Department = department;
            NameDepartment = Department.Name;
            CloseWindow = closeWindow;
        }

        public ICommand Edit => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();

            var findDepartment = db.Departments.FirstOrDefault(department => department.Id_department == Department.Id_department);

            if (findDepartment == null)
                MessageBox.Show("Объект не найдет в базе данных", "Ошибка при изменении отдела", MessageBoxButton.OK);
            else
            {
                findDepartment.Name = NameDepartment;
                db.SaveChanges();
            }

            CloseWindow();
        }, () => !string.IsNullOrWhiteSpace(NameDepartment));

        public ICommand Cancel => new DelegateCommand(() => CloseWindow());
    }
}
