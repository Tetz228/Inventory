namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
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

        public ICommand AddEmployee => new DelegateCommand(() =>
        {
            foreach (var postsEmployee in PostsEmployees)
            {
                int fk = postsEmployee.Fk_post;
            }
        });

        public ICommand AddPost => new DelegateCommand(() =>
        {
            PostsEmployees.Add(new Posts_employees());
        });

        public ICommand DeletePost => new DelegateCommand<Posts_employees>((postEmp) => PostsEmployees.Remove(postEmp));

        public ICommand AddDepartment => new DelegateCommand(() =>
        {
            EmployeesInDepartments.Add(new Employees_in_departments());
        });

        public ICommand DeleteDepartment => new DelegateCommand<Employees_in_departments>((employeesInDepartments) => EmployeesInDepartments.Remove(employeesInDepartments));
    }
}
