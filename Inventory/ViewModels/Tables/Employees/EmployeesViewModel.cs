namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class EmployeesViewModel : BindableBase
    {
        public EmployeesViewModel()
        {
            using var db = new InventoryEntities();
            Employees = new ObservableCollection<Employee>(db.Employees.Include(employeePost => employeePost.Posts_employees
                                                                       .Select(post => post.Post))
                                                                       .Include(empDepart => empDepart.Employees_in_departments
                                                                       .Select(depart => depart.Department)));
            Employees.Sort(employee => employee.L_name, SortDirection = ListSortDirection.Ascending);
            EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);
        }

        #region Свойства

        private ICollectionView EmployeesCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Employee> Employees { get; set; }

        public Employee SelectEmployee { get; set; }

        private string _employeesFilter = string.Empty;

        public string EmployeesFilter
        {
            get => _employeesFilter;
            set
            {
                _employeesFilter = value;
                EmployeesCollection.Filter = obj =>
                {
                    if (obj is Employee employee)
                        return Employee.SearchFor(employee, EmployeesFilter);

                    return false;
                };
                EmployeesCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "ФИО":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Employees.Sort(employee => employee.L_name, SortDirection = ListSortDirection.Descending);
                            else
                                Employees.Sort(employee => employee.L_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Почта":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Employees.Sort(employee => employee.Email, SortDirection = ListSortDirection.Descending);
                            else
                                Employees.Sort(employee => employee.Email, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Номер телефона":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Employees.Sort(employee => employee.Phone_number, SortDirection = ListSortDirection.Descending);
                            else
                                Employees.Sort(employee => employee.Phone_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                        //case "Должности":
                        //    {
                        //        if (SortDirection == ListSortDirection.Ascending)
                        //            Employees.Sort(employee => employee.Posts_employees, SortDirection = ListSortDirection.Descending);
                        //        else
                        //            Employees.Sort(employee => employee.Posts_employees, SortDirection = ListSortDirection.Ascending);
                        //        break;
                        //    }
                        //case "Отделы":
                        //    {
                        //        if (SortDirection == ListSortDirection.Ascending)
                        //            Employees.Sort(employee => employee.Employees_in_departments, SortDirection = ListSortDirection.Descending);
                        //        else
                        //            Employees.Sort(employee => employee.Employees_in_departments, SortDirection = ListSortDirection.Ascending);
                        //        break;
                        //    }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectEmployee = null;
        #endregion

        #region Команды
        public ICommand AddEmployeeCommand => new DelegateCommand(() =>
        {
            var addEmployeeWindow = new EmployeeAddWindow();
            addEmployeeWindow.ShowDialog();
        });

        public ICommand EditEmployeeCommand => new DelegateCommand<Employee>(employee =>
        {
            using var db = new InventoryEntities();

            var editEmployeeViewModel = new EmployeeEditViewModel(employee);
            Employee.EmployeesInDepartments = new ObservableCollection<Employees_in_departments>(employee.Employees_in_departments);
            Employee.PostsEmployees = new ObservableCollection<Posts_employees>(employee.Posts_employees);

            var editEmployeeWindow = new EmployeeEditWindow();
            editEmployeeWindow.DataContext = editEmployeeViewModel;
            editEmployeeWindow.Closing += editEmployeeViewModel.OnWindowClosing;
            editEmployeeWindow.ShowDialog();

        }, employee => employee != null);

        public ICommand DeleteEmployeeCommand => new DelegateCommand<Employee>(selectEmployee =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectEmployee.L_name} {selectEmployee.F_name} {selectEmployee.M_name} {selectEmployee.Email}?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            int id = selectEmployee.Id_employee;
            Employee.DeleteEmployee(selectEmployee);
            //Services.Delete<Employee>(selectEmployee.Id_employee);
            //Dele
            Employee.RefreshCollection();
        }, selectHdd => selectHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Employee.RefreshCollection);
        #endregion
    }
}