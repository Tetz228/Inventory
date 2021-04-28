namespace Inventory.Model
{
    using DevExpress.Mvvm;

    public partial class Employees_in_departments:BindableBase
    {
        public int Id_employee_in_department { get; set; }
        public int Fk_department { get; set; }
        public int Fk_employee { get; set; }

        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
