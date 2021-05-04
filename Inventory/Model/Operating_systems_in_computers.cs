namespace Inventory.Model
{
    public partial class Operating_systems_in_computers
    {
        public int Id_operating_systems_in_computers { get; set; }
        public int Fk_operating_system { get; set; }
        public int Fk_computer { get; set; }
    
        public virtual Computer Computer { get; set; }
        public virtual Operating_systems Operating_systems { get; set; }
    }
}
