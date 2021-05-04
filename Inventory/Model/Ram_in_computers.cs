namespace Inventory.Model
{
    public partial class Ram_in_computers
    {
        public int Id_ram_in_computer { get; set; }
        public int Fk_inventory_number_ram { get; set; }
        public int Fk_computer { get; set; }
    
        public virtual Computer Computer { get; set; }
        public virtual Inventory_numbers_ram Inventory_numbers_ram { get; set; }
    }
}
