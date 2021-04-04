namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory_numbers_power_supplies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_power_supplies()
        {
            this.Computers = new HashSet<Computer>();
        }
    
        public int Id_inventory_number_power_supplie { get; set; }
        public int Fk_power_supplie { get; set; }
        public int Inventory_number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Computer> Computers { get; set; }
        public virtual Power_supplies Power_supplies { get; set; }
    }
}
