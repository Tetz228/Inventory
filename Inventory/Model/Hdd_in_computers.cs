namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hdd_in_computers
    {
        public int Id_hdd_in_computer { get; set; }
        public int Fk_inventory_number_hdd { get; set; }
        public int Fk_computer { get; set; }
    
        public virtual Computer Computer { get; set; }
        public virtual Inventory_numbers_hdd Inventory_numbers_hdd { get; set; }
    }
}
