namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory_numbers_processors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_processors()
        {
            this.Processors_in_computers = new HashSet<Processors_in_computers>();
        }
    
        public int Id_inventory_number_processor { get; set; }
        public int Fk_processor { get; set; }
        public int Inventory_number { get; set; }
    
        public virtual Processor Processor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Processors_in_computers> Processors_in_computers { get; set; }
    }
}
