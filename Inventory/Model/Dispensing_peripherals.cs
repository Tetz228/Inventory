namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dispensing_peripherals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dispensing_peripherals()
        {
            this.List_dispensed_peripherals = new HashSet<List_dispensed_peripherals>();
        }
    
        public int Id_dispensing_peripheral { get; set; }
        public System.DateTime Date_dispensing { get; set; }
        public int Fk_user { get; set; }
        public int Fk_employee { get; set; }
        public string Note { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<List_dispensed_peripherals> List_dispensed_peripherals { get; set; }
    }
}
