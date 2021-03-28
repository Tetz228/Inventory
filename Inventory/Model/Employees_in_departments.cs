namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Employees_in_departments
    {
        public Employees_in_departments()
        {
            using var db = new InventoryEntities();
            CollectionDepartments = new List<Department>(db.Departments);
        }

        public static List<Department> CollectionDepartments { get; set; }

        public int Id_employee_in_department { get; set; }
        public int Fk_department { get; set; }
        public int Fk_employee { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }

        public static void AddEmployeeInDepartment(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var department in Employee.EmployeesInDepartments)
                department.Fk_employee = idEmployee;

            db.Employees_in_departments.AddRange(Employee.EmployeesInDepartments);
            db.SaveChanges();
        }

        public static void EditEmployeeInDepartment(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var department in Employee.EmployeesInDepartments)
            {
                if (department.Id_employee_in_department == 0)
                {
                    department.Fk_employee = idEmployee;
                    db.Employees_in_departments.Add(department);
                    db.SaveChanges();
                }
                else
                {
                    var employeesInDepartments = db.Employees_in_departments.Where(empDepart => empDepart.Id_employee_in_department == department.Id_employee_in_department).ToList();
                    foreach (var item in employeesInDepartments)
                    {
                        item.Fk_department = department.Fk_department;
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void DeleteEmployeeDepartment(Employees_in_departments selectEmpInDepart)
        {
            if (selectEmpInDepart.Id_employee_in_department != 0)
            {
                using var db = new InventoryEntities();
                var foundEmpDepart = db.Employees_in_departments.FirstOrDefault(inDepartments => inDepartments.Id_employee_in_department == selectEmpInDepart.Id_employee_in_department);

                if (foundEmpDepart != null)
                {
                    db.Employees_in_departments.Remove(foundEmpDepart);
                    db.SaveChanges();
                }
            }

            Employee.EmployeesInDepartments.Remove(selectEmpInDepart);
        }

        public static void DeleteEmployeeDepartment(int idEmployee)
        {
            using var db = new InventoryEntities();

            var depEmp = db.Employees_in_departments.Where(emp => emp.Fk_employee == idEmployee);

            db.Employees_in_departments.RemoveRange(depEmp);
            db.SaveChanges();
        }
    }
}
