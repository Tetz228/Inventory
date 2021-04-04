namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class History_dispensed_peripherals
    {
        public int Id_history_dispensed_peripheral { get; set; }
        public int Number_dispensing { get; set; }
        public string Operation { get; set; }
        public int Peripheral { get; set; }
        public System.DateTime CreateAt { get; set; }
    }
}
