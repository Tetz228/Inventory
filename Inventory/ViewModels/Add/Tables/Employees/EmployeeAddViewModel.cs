namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeAddViewModel : BindableBase
    {
        #region Свойства
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ObservableCollection<Posts_employees> PostsEmployees { get; set; } = new();

        public ObservableCollection<Employees_in_departments> EmployeesInDepartments { get; set; } = new();
        #endregion

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                return false;

            if (string.IsNullOrWhiteSpace(FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(MiddleName))
                return false;

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;

            if (string.IsNullOrWhiteSpace(Email))
                return false;

            if (PostsEmployees.Count == 0)
                return false;

            if (EmployeesInDepartments.Count == 0)
                return false;

            return true;
        }

        #region Команды
        public ICommand AddEmployee => new DelegateCommand<Window>(empAddWindow=>
        {
            using var db = new InventoryEntities();

            var employee = new Employee
            {
                L_name = LastName,
                F_name = FirstName,
                M_name = MiddleName,
                Phone_number = PhoneNumber,
                Email = this.Email
            };

            db.Employees.Add(employee);
            db.SaveChanges();

            foreach (var post in PostsEmployees)
                post.Fk_employee = employee.Id_employee;

            db.Posts_employees.AddRange(PostsEmployees);
            db.SaveChanges();

            foreach (var department in EmployeesInDepartments)
                department.Fk_employee = employee.Id_employee;

            db.Employees_in_departments.AddRange(EmployeesInDepartments);
            db.SaveChanges();

            empAddWindow.Close();

        },_=> Validation());

        public ICommand Cancel => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddPostInCollection => new DelegateCommand(() => PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>((postEmp) => PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>((empInDepart) => EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}