namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory_numbers_ram
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_ram()
        {
            this.Ram_in_computers = new HashSet<Ram_in_computers>();
        }
    
        public int Id_inventory_number_ram { get; set; }
        public int Fk_ram { get; set; }
        public int Inventory_number { get; set; }
    
        public virtual Ram Ram { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ram_in_computers> Ram_in_computers { get; set; }
    }
}
