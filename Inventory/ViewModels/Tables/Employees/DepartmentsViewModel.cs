namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DepartmentsViewModel : BaseViewModel<Department>
    {
        public DepartmentsViewModel() : base(Departments) => RefreshCollection();

        public static ObservableCollection<Department> Departments { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            Departments.Sort(department => department.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddDepartmentCommand => new DelegateCommand(() =>
        {
            var addDepartmentWindow = new DepartmentAddWindow();
            addDepartmentWindow.ShowDialog();
        });

        public ICommand EditDepartmentCommand => new DelegateCommand<Department>(department =>
        {
            var editDepartmentWindow = new DepartmentEditWindow();
            var editDepartmentViewModel = new DepartmentEditViewModel(department);
            editDepartmentWindow.DataContext = editDepartmentViewModel;
            editDepartmentWindow.Closing += editDepartmentViewModel.OnWindowClosing;
            editDepartmentWindow.ShowDialog();
        }, depart => depart != null);

        public ICommand DeleteDepartmentCommand => new DelegateCommand<Department>(selectDepartment =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить отдел:\nнаименование - {selectDepartment.Name}?", "Удаление отдела", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Department>(selectDepartment.Id_department))
                Departments.Remove(selectDepartment);
        }, selectDepartment => selectDepartment != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);

        private static void RefreshCollection()
        {
            Departments.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Departments.AsNoTracking())
            {
                Departments.Add(item);
            }
            Departments.Sort(department => department.Name);
        }
    }
}