namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dispensing_computers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dispensing_computers()
        {
            this.List_dispensed_computers = new HashSet<List_dispensed_computers>();
        }
    
        public int Id_dispensing_computer { get; set; }
        public System.DateTime Date_dispensing { get; set; }
        public int Fk_user { get; set; }
        public int Fk_employee { get; set; }
        public string Note { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<List_dispensed_computers> List_dispensed_computers { get; set; }
    }
}
