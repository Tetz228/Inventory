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

    using DevExpress.Mvvm;

    public partial class Employees_in_departments : BindableBase
    {
        public Employees_in_departments()
        {
            using var db = new InventoryEntities();
        }

        public int Id_employee_in_department { get; set; }
        public int Fk_department { get; set; }
        public int Fk_employee { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
