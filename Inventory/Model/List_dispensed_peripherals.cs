namespace Inventory.Model
{
    public partial class List_dispensed_peripherals
    {
        public int Id_list_dispensed_peripheral { get; set; }
        public int Fk_number_dispensing { get; set; }
        public int Fk_inventory_number_peripheral { get; set; }
    
        public virtual Dispensing_peripherals Dispensing_peripherals { get; set; }
        public virtual Inventory_numbers_peripherals Inventory_numbers_peripherals { get; set; }
    }
}
