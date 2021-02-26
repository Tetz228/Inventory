namespace Inventory.ViewModels.Add
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class DepartmentAddViewModel : BindableBase
    {
        public string DepartmentName { get; set; }

        public ICommand Add => new DelegateCommand<Window>(addWindow =>
        {
            using var db = new InventoryEntities();

            var department = new Department
            {
                Name = DepartmentName
            };

            db.Departments.Add(department);
            db.SaveChanges();

            addWindow.Close();
        }, _ => !string.IsNullOrWhiteSpace(DepartmentName));

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
    }
}
