namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;

    using DevExpress.Mvvm;

    public partial class Employees_in_departments : BindableBase
    {
        public Employees_in_departments()
        {
            using var db = new InventoryEntities();
            CollectionDepartments = new List<Department>(db.Departments);
        }

        public List<Department> CollectionDepartments { get; set; }

        public int Id_employee_in_department { get; set; }
        public int Fk_department { get; set; }
        public int Fk_employee { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
