namespace Inventory.Model
{
    public partial class List_dispensed_computers
    {
        public int Id_list_dispensed_computer { get; set; }
        public int Fk_number_dispensing { get; set; }
        public int Fk_computer { get; set; }
    
        public virtual Computer Computer { get; set; }
        public virtual Dispensing_computers Dispensing_computers { get; set; }
    }
}
