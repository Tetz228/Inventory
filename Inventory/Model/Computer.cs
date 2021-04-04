namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Computer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Computer()
        {
            this.Hdd_in_computers = new HashSet<Hdd_in_computers>();
            this.List_dispensed_computers = new HashSet<List_dispensed_computers>();
            this.Operating_systems_in_computers = new HashSet<Operating_systems_in_computers>();
            this.Processors_in_computers = new HashSet<Processors_in_computers>();
            this.Ram_in_computers = new HashSet<Ram_in_computers>();
            this.Ssd_in_computers = new HashSet<Ssd_in_computers>();
        }
    
        public int Id_computer { get; set; }
        public int Fk_inventory_number_motherboard { get; set; }
        public Nullable<int> Fk_inventory_number_graphics_card { get; set; }
        public int Fk_inventory_number_power_supplies { get; set; }
        public string Ip_address { get; set; }
        public int Inventory_number { get; set; }
        public int Fk_status_computer { get; set; }
    
        public virtual Inventory_numbers_graphics_cards Inventory_numbers_graphics_cards { get; set; }
        public virtual Inventory_numbers_motherboards Inventory_numbers_motherboards { get; set; }
        public virtual Inventory_numbers_power_supplies Inventory_numbers_power_supplies { get; set; }
        public virtual Statuses_computers Statuses_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hdd_in_computers> Hdd_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<List_dispensed_computers> List_dispensed_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operating_systems_in_computers> Operating_systems_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Processors_in_computers> Processors_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ram_in_computers> Ram_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ssd_in_computers> Ssd_in_computers { get; set; }
    }
}
