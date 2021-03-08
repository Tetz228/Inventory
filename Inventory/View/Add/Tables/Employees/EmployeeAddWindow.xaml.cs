namespace Inventory.View.Add.Tables.Employees
{
    using Inventory.ViewModels.Add.Tables.Employees;
    using System.Windows;

    public partial class EmployeeAddWindow : Window
    {
        public EmployeeAddWindow()
        {
            InitializeComponent();

            var employeeAddViewModel = new EmployeeAddViewModel();

            DataContext = employeeAddViewModel;
            Closing += employeeAddViewModel.OnWindowClosing;
        }
    }
}
