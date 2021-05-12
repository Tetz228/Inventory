namespace Inventory.Model
{
    public partial class History_dispensed_computers
    {
        public int Id_history_dispensed_computer { get; set; }
        public int Number_dispensing { get; set; }
        public string Operation { get; set; }
        public int Computer { get; set; }
        public System.DateTime CreateAt { get; set; }
    }
}
