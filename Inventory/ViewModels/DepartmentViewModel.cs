using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View;

    class DepartmentViewModel:BindableBase
    {
        public ObservableCollection<Department> Departments { get; set; }

        public Department SelectDepartment { get; set; }

        public DepartmentViewModel()
        {
            using var db = new InventoryEntities();
            Departments = new ObservableCollection<Department>(db.Departments.ToList());
        }

        public ICommand DataGridMouseLeftButtonDown => new DelegateCommand(() =>
        {
            SelectDepartment = null;
        });

        public ICommand AddDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();

            var addDepartmentWindow = new DepartmentAddWindow();
            addDepartmentWindow.DataContext = new DepartmentAddViewModel(addDepartmentWindow.Close);
            addDepartmentWindow.ShowDialog();

            Departments = new ObservableCollection<Department>(db.Departments.ToList());
        });

        public ICommand EditDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();

            var editDepartmentWindow = new DepartmentEditWindow();
            editDepartmentWindow.DataContext = new DepartmentEditViewModel(SelectDepartment, editDepartmentWindow.Close);
            editDepartmentWindow.ShowDialog();

            Departments = new ObservableCollection<Department>(db.Departments.ToList());
        }, () => SelectDepartment != null);

        public ICommand DeleteDepartment => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();
            
        }, () => SelectDepartment != null);
    }
}
