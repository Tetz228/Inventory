//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory_numbers_ssd
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_ssd()
        {
            this.Ssd_in_computers = new HashSet<Ssd_in_computers>();
        }
    
        public int Id_inventory_number_ssd { get; set; }
        public int Fk_ssd { get; set; }
        public int Inventory_number { get; set; }
    
        public virtual Ssd Ssd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ssd_in_computers> Ssd_in_computers { get; set; }
    }
}
