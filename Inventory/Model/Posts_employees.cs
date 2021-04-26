namespace Inventory.Model
{
    public partial class Posts_employees
    {
        public int Id_post_employee { get; set; }
        public int Fk_employee { get; set; }
        public int Fk_post { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Post Post { get; set; }
    }
}
