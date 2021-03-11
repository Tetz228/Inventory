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

            EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);
            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.L_name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Employee> Employees { get; set; }

        public ICollectionView EmployeesCollection { get; }

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
                        return Employee.Search(employee, EmployeesFilter);

                    return false;
                };
                EmployeesCollection.Refresh();
            }
        }
        #endregion

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "ФИО":
                    {
                        if (EmployeesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.L_name), ListSortDirection.Descending));
                        }
                        else
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.L_name), ListSortDirection.Ascending));
                        }

                        EmployeesCollection.Refresh();

                        break;
                    }
                case "Почта":
                    {
                        if (EmployeesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Email), ListSortDirection.Descending));
                        }
                        else
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Email), ListSortDirection.Ascending));
                        }

                        EmployeesCollection.Refresh();

                        break;
                    }
                case "Номер телефона":
                    {
                        if (EmployeesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Phone_number), ListSortDirection.Descending));
                        }
                        else
                        {
                            EmployeesCollection.SortDescriptions.Clear();
                            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Phone_number), ListSortDirection.Ascending));
                        }

                        EmployeesCollection.Refresh();

                        break;
                    }
                #region Работает криво
                //case "Должности":
                //    {
                //        if (EmployeesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                //        {
                //            EmployeesCollection.SortDescriptions.Clear();
                //            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Posts_employees), ListSortDirection.Descending));
                //        }
                //        else
                //        {
                //            EmployeesCollection.SortDescriptions.Clear();
                //            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Posts_employees), ListSortDirection.Ascending));
                //        }

                //        EmployeesCollection.Refresh();

                //        break;
                //    }
                //case "Отделы":
                //    {
                //        if (EmployeesCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                //        {
                //            EmployeesCollection.SortDescriptions.Clear();
                //            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Employees_in_departments), ListSortDirection.Descending));
                //        }
                //        else
                //        {
                //            EmployeesCollection.SortDescriptions.Clear();
                //            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.Employees_in_departments), ListSortDirection.Ascending));
                //        }

                //        EmployeesCollection.Refresh();
                //        break;
                //    }

                    #endregion
            }
        }

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectEmployee = null);

        public ICommand AddEmployee => new DelegateCommand(() =>
        {
            var addEmployeeWindow = new EmployeeAddWindow();

            addEmployeeWindow.ShowDialog();
        });

        public ICommand EditEmployee => new DelegateCommand<Employee>(employee =>
        {
            using var db = new InventoryEntities();

            var editEmployeeViewModel = new EmployeeEditViewModel(employee);
            Employee.EmployeesInDepartments = new ObservableCollection<Employees_in_departments>(employee.Employees_in_departments);
            Employee.PostsEmployees = new ObservableCollection<Posts_employees>(employee.Posts_employees);

            var editEmployeeWindow = new EmployeeEditWindow
            {
                DataContext = editEmployeeViewModel
            };

            editEmployeeWindow.Closing += editEmployeeViewModel.OnWindowClosing;
            editEmployeeWindow.ShowDialog();

        }, employee => employee != null);

        public ICommand DeleteEmployee => new DelegateCommand<Employee>(Employee.DeleteEmployee, selectEmployee => selectEmployee != null);

        public ICommand RefreshList => new DelegateCommand(Employee.RefreshCollection);
        #endregion
    }
}