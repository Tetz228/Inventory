namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ssd_in_computers
    {
        public int Id_ssd_in_computer { get; set; }
        public int Fk_inventory_number_ssd { get; set; }
        public int Fk_computer { get; set; }
    
        public virtual Computer Computer { get; set; }
        public virtual Inventory_numbers_ssd Inventory_numbers_ssd { get; set; }
    }
}
