namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public partial class Computer : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Computer()
        {
            Hdd_in_computers = new ObservableCollection<Hdd_in_computers>();
            List_dispensed_computers = new ObservableCollection<List_dispensed_computers>();
            Operating_systems_in_computers = new ObservableCollection<Operating_systems_in_computers>();
            Processors_in_computers = new ObservableCollection<Processors_in_computers>();
            Ram_in_computers = new ObservableCollection<Ram_in_computers>();
            Ssd_in_computers = new ObservableCollection<Ssd_in_computers>();
        }

        public int Id_computer { get; set; }
        public int Fk_inventory_number_motherboard { get; set; }
        public int? Fk_inventory_number_graphics_card { get; set; }
        public int Fk_inventory_number_power_supplies { get; set; }
        public string Ip_address { get; set; }
        public int Inventory_number { get; set; }
        public int Fk_status_computer { get; set; }

        public virtual Inventory_numbers_graphics_cards Inventory_numbers_graphics_cards { get; set; }
        public virtual Inventory_numbers_motherboards Inventory_numbers_motherboards { get; set; }
        public virtual Inventory_numbers_power_supplies Inventory_numbers_power_supplies { get; set; }
        public virtual Statuses_computers Statuses_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Hdd_in_computers> Hdd_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<List_dispensed_computers> List_dispensed_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Operating_systems_in_computers> Operating_systems_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Processors_in_computers> Processors_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Ram_in_computers> Ram_in_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Ssd_in_computers> Ssd_in_computers { get; set; }

        #region ���������
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Inventory_number":
                        if (Inventory_number <= 0)
                            result = "����� ������ ���� ������ 0";
                        else if (Services.CheckForUniqueness<Computer>(nameof(Inventory_number), Inventory_number, _selectComputer?.Inventory_number))
                            result = "����� ������ ���� ����������";
                        break;
                    case "Ip_address":
                        if (string.IsNullOrWhiteSpace(Ip_address))
                            result = "���� �� ������ ���� ������";
                        else if (IsValidationIpAddress(Ip_address) == false)
                            result = "������������ IP-�����";
                        else if (Services.CheckForUniqueness<Computer>(nameof(Ip_address), Ip_address, _selectComputer?.Ip_address))
                            result = "����� ������ ���� ����������";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }
        #endregion

        private bool IsValidationIpAddress(string ipAddress) => Regex.IsMatch(ipAddress, @"([0-9]{1,3}[\.]){3}[0-9]{1,3}");

        #region ����� ���������
        private Computer _selectComputer;

        public void BeginEdit()
        {
            _selectComputer = new Computer
            {
                Id_computer = Id_computer,
                Fk_inventory_number_motherboard = Fk_inventory_number_motherboard,
                Fk_inventory_number_graphics_card = Fk_inventory_number_graphics_card,
                Fk_inventory_number_power_supplies = Fk_inventory_number_power_supplies,
                Ip_address = Ip_address,
                Inventory_number = Inventory_number,
                Fk_status_computer = Fk_status_computer,
                Hdd_in_computers = Hdd_in_computers.CopyingElements(),
                Operating_systems_in_computers = Operating_systems_in_computers.CopyingElements(),
                Processors_in_computers = Processors_in_computers.CopyingElements(),
                Ram_in_computers = Ram_in_computers.CopyingElements(),
                Ssd_in_computers = Ssd_in_computers.CopyingElements()
            };
        }

        public void EndEdit() => _selectComputer = null;

        public void CancelEdit()
        {
            if (_selectComputer == null)
                return;

            Id_computer = _selectComputer.Id_computer;
            Fk_inventory_number_motherboard = _selectComputer.Fk_inventory_number_motherboard;
            Fk_inventory_number_graphics_card = _selectComputer.Fk_inventory_number_graphics_card;
            Fk_inventory_number_power_supplies = _selectComputer.Fk_inventory_number_power_supplies;
            Ip_address = _selectComputer.Ip_address;
            Inventory_number = _selectComputer.Inventory_number;
            Fk_status_computer = _selectComputer.Fk_status_computer;
            Hdd_in_computers = _selectComputer.Hdd_in_computers;
            Operating_systems_in_computers = _selectComputer.Operating_systems_in_computers;
            Processors_in_computers = _selectComputer.Processors_in_computers;
            Ram_in_computers = _selectComputer.Ram_in_computers;
            Ssd_in_computers = _selectComputer.Ssd_in_computers;
        }
        #endregion
    }
}
