namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public partial class Dispensing_peripherals : BindableBase, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dispensing_peripherals() => List_dispensed_peripherals = new ObservableCollection<List_dispensed_peripherals>();

        public int Id_dispensing_peripheral { get; set; }
        public DateTime Date_dispensing { get; set; } = DateTime.Now.Date;
        public int Fk_user { get; set; } = User.AuthorizedUser.Id_user;
        public int Fk_employee { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<List_dispensed_peripherals> List_dispensed_peripherals { get; set; }

        #region Откат изменений
        private Dispensing_peripherals _selectDispensing;

        public void BeginEdit()
        {
            _selectDispensing = new Dispensing_peripherals
            {
                Id_dispensing_peripheral = Id_dispensing_peripheral,
                Date_dispensing = Date_dispensing,
                Fk_user = Fk_user,
                Fk_employee = Fk_employee
            };
        }

        public void EndEdit() => _selectDispensing = null;

        public void CancelEdit()
        {
            if (_selectDispensing == null)
                return;

            Id_dispensing_peripheral = _selectDispensing.Id_dispensing_peripheral;
            Date_dispensing = _selectDispensing.Date_dispensing;
            Fk_user = _selectDispensing.Fk_user;
            Fk_employee = _selectDispensing.Fk_employee;
        }
        #endregion
    }
}
