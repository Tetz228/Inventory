namespace Inventory.ViewModels
{
    using System;

    using DevExpress.Mvvm;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Pages;

    class TablesEmployeesViewModel : BindableBase
    {
        public Page CurrentPage { get; set; }

        public DepartamentsPage DepartamentsPage { get; set; }
        public EmployeesPage EmployeesPage { get; set; }
        public PostsPage PostsPage { get; set; }

        public ICommand SelectTableDepartament => new DelegateCommand(() =>
        {
            CurrentPage = DepartamentsPage;
        });

        public ICommand SelectTableEmployee => new DelegateCommand(() =>
        {
            CurrentPage = EmployeesPage;
        });

        public ICommand SelectTablePost => new DelegateCommand(() =>
        {
            CurrentPage = PostsPage;
        });

        public TablesEmployeesViewModel()
        {
            DepartamentsPage = new DepartamentsPage();
            EmployeesPage = new EmployeesPage();
            PostsPage = new PostsPage();
        }
    }
}
