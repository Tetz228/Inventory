using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class EmployeeAddViewModel:BindableBase
    {
        public Employee Employee { get; set; }
        public static ObservableCollection<Post> Posts { get; set; }
        public static ObservableCollection<Post> AddPosts { get; set; }

        public string KeyWord { get; set; }

        public EmployeeAddViewModel()
        {
            var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts);
            Employee = new Employee();
        }

        public ICommand AddCommand => new DelegateCommand(()=>
        {
            Employee.Posts_employees1.Add(new Posts_employees());
        });

        public string Value { get; set; }
    }
}
