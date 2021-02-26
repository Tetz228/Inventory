namespace Inventory.ViewModels.Edit
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class DepartmentEditViewModel : BindableBase
    {
        public Department Department { get; set; }

        public ICommand Edit => new DelegateCommand<Window>(editWindow =>
        {
            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == Department.Id_department);

            if (findDepartment == null)
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении отдела", MessageBoxButton.OK,MessageBoxImage.Error);
            else
            {
                findDepartment.Name = Department.Name;
                db.SaveChanges();
            }
            editWindow.Close();
        }, _ => !string.IsNullOrWhiteSpace(Department.Name));

        public ICommand Cancel => new DelegateCommand<Window>(editWindow => editWindow.Close());
    }
}
