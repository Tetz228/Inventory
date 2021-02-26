namespace Inventory.Model
{
    using System.Collections.Generic;

    using DevExpress.Mvvm;

    public partial class Department:BindableBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
        }
    
        public int Id_department { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_in_departments> Employees_in_departments { get; set; }
    }
}
