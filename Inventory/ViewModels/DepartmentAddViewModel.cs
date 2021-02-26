namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class DepartmentAddViewModel : BindableBase
    {
        public string DepartmentName { get; set; }

        public ICommand Add => new DelegateCommand<Window>(editWindow =>
        {
            using var db = new InventoryEntities();

            var department = new Department
            {
                Name = DepartmentName
            };

            db.Departments.Add(department);
            db.SaveChanges();

            editWindow.Close();
        }, _ => !string.IsNullOrWhiteSpace(DepartmentName));

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
    }
}
