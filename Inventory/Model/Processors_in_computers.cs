namespace Inventory.Model
{
    using DevExpress.Mvvm;

    public partial class Processors_in_computers: BindableBase
    {
        public int Id_processor_in_computer { get; set; }
        public int Fk_inventory_number_processor { get; set; }
        public int Fk_computer { get; set; }

        public virtual Computer Computer { get; set; }
        public virtual Inventory_numbers_processors Inventory_numbers_processors { get; set; }
    }
}
