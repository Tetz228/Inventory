namespace Inventory.Services
{
    using Inventory.Model;
    using System.Globalization;
    using System.Linq;

    public static class SearchFor
    {
        #region Peripherals

        public static bool Search(this Inventory_numbers_peripherals inventoryPeripheral, string inventoryPeripheralFilter) => inventoryPeripheral.Inventory_number.ToString().ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Types_peripherals.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Manufacturer.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Statuses_peripherals.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower());

        public static bool Search(this Peripheral peripheral, string peripheralFilter) => peripheral.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Types_peripherals.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Manufacturer.Name.ToLower().Contains(peripheralFilter.ToLower());

        public static bool Search(this Types_peripherals typePeripheral, string typePeripheralFilter) => typePeripheral.Name.ToLower().Contains(typePeripheralFilter.ToLower());

        public static bool Search(this Statuses_peripherals statusPeripheral, string statusPeripheralFilter) => statusPeripheral.Name.ToLower().Contains(statusPeripheralFilter.ToLower());

        #endregion

        #region Employees

        public static bool Search(this Department department, string departmentsFilter) => department.Name.ToLower().Contains(departmentsFilter.ToLower());

        public static bool Search(this Employee employee, string employeesFilter) => employee.L_name.ToLower().Contains(employeesFilter.ToLower())
            || employee.F_name.ToLower().Contains(employeesFilter.ToLower())
            || employee.Email.ToLower().Contains(employeesFilter.ToLower())
            || employee.Phone_number.ToLower().Contains(employeesFilter.ToLower())
            || employee.Posts_employees.Select(postsEmployees => postsEmployees.Post.Name.ToLower().Contains(employeesFilter.ToLower())).FirstOrDefault()
            || employee.Employees_in_departments.Select(employeesInDepartments => employeesInDepartments.Department.Name.ToLower().Contains(employeesFilter.ToLower())).FirstOrDefault();

        public static bool Search(this Post post, string postsFilter) => post.Name.ToLower().Contains(postsFilter.ToLower());

        public static bool Search(this User user, string usersFilter) => user.Login.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.L_name.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.F_name.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.Email.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Role.Name.ToLower().Contains(usersFilter.ToLower());

        #endregion

        #region Computers

        #region Accessories

        public static bool Search(this Graphics_cards graphicCard, string graphicCardFilter) => graphicCard.Name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                                                     || graphicCard.Unit.Full_name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                                                     || graphicCard.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(graphicCardFilter.ToLower())
                                                                                                     || graphicCard.Unit.Short_name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                                                     || graphicCard.Manufacturer.Name.ToLower().Contains(graphicCardFilter.ToLower());

        public static bool Search(this Hdd hdd, string hddFilter) => hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Unit.Full_name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Unit.Short_name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Types_hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Manufacturer.Name.ToLower().Contains(hddFilter.ToLower());

        public static bool Search(this Ssd ssd, string ssdFilter) => ssd.Name.ToLower().Contains(ssdFilter.ToLower())
                                                                     || ssd.Unit.Full_name.ToLower().Contains(ssdFilter.ToLower())
                                                                     || ssd.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(ssdFilter.ToLower())
                                                                     || ssd.Unit.Short_name.ToLower().Contains(ssdFilter.ToLower())
                                                                     || ssd.Types_ssd.Name.ToLower().Contains(ssdFilter.ToLower())
                                                                     || ssd.Manufacturer.Name.ToLower().Contains(ssdFilter.ToLower());

        public static bool Search(this Power_supplies powerSupply, string powerSuppliesFilter) => powerSupply.Name.ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                     || powerSupply.Unit.Full_name.ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                     || powerSupply.Power.ToString().ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                     || powerSupply.Unit.Short_name.ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                     || powerSupply.Manufacturer.Name.ToLower().Contains(powerSuppliesFilter.ToLower());

        public static bool Search(this Processor processor, string processorsFilter) => processor.Name.ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Unit.Full_name.ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Base_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Socket.Name.ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Amount_cores.ToString().ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Unit.Short_name.ToLower().Contains(processorsFilter.ToLower())
                                                                                        || processor.Manufacturer.Name.ToLower().Contains(processorsFilter.ToLower());

        public static bool Search(this Motherboard motherboard, string motherboardFilter) => motherboard.Name.ToLower().Contains(motherboardFilter.ToLower())
            || motherboard.Socket.Name.ToLower().Contains(motherboardFilter.ToLower())
            || motherboard.Manufacturer.Name.ToLower().Contains(motherboardFilter.ToLower());

        public static bool Search(this Ram ram, string ramsFilter) => ram.Name.ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Unit.Full_name.ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Clock_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Types_memory.Name.ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Unit.Short_name.ToLower().Contains(ramsFilter.ToLower())
                                                                      || ram.Manufacturer.Name.ToLower().Contains(ramsFilter.ToLower());

        #endregion

        #region Computers

        public static bool Search(this Operating_systems operatingSystem, string operatingSystemsFilter) => operatingSystem.Name.ToLower().Contains(operatingSystemsFilter.ToLower())
            || operatingSystem.System_version.ToLower().Contains(operatingSystemsFilter.ToLower());

        public static bool Search(this Statuses_computers statusComputer, string statusComputerFilter) => statusComputer.Name.ToLower().Contains(statusComputerFilter.ToLower());

        public static bool Search(this Computer computer, string computerFilter) => computer.Inventory_number.ToString().ToLower().Contains(computerFilter.ToLower())
                                                                                 || computer.Ip_address.ToLower().Contains(computerFilter.ToLower())
                                                                                 || computer.Statuses_computers.Name.ToLower().Contains(computerFilter.ToLower());

        #endregion

        #region Inventory numbers

        public static bool Search(this Inventory_numbers_motherboards inventoryMotherboard, string inventoryMotherboardFilter) => inventoryMotherboard.Inventory_number.ToString().ToLower().Contains(inventoryMotherboardFilter.ToLower())
            || inventoryMotherboard.Motherboard.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower())
            || inventoryMotherboard.Motherboard.Manufacturer.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower())
            || inventoryMotherboard.Motherboard.Socket.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower());


        public static bool Search(this Inventory_numbers_hdd inventoryHdd, string inventoryHddFilter) => inventoryHdd.Inventory_number.ToString().ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Types_hdd.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Manufacturer.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Unit.Full_name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Unit.Short_name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                                    || inventoryHdd.Hdd.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryHddFilter.ToLower());

        public static bool Search(this Inventory_numbers_ssd inventorySsd, string inventorySsdFilter) => inventorySsd.Inventory_number.ToString().ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Name.ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Types_ssd.Name.ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Manufacturer.Name.ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Unit.Full_name.ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Unit.Short_name.ToLower().Contains(inventorySsdFilter.ToLower())
            || inventorySsd.Ssd.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventorySsdFilter.ToLower());

        public static bool Search(this Inventory_numbers_graphics_cards inventoryNumberHdd, string inventoryNumberHddFilter) => inventoryNumberHdd.Inventory_number.ToString().ToLower().Contains(inventoryNumberHddFilter.ToLower())
                                                                                                                                || inventoryNumberHdd.Graphics_cards.Name.ToLower().Contains(inventoryNumberHddFilter.ToLower())
                                                                                                                                || inventoryNumberHdd.Graphics_cards.Manufacturer.Name.ToLower().Contains(inventoryNumberHddFilter.ToLower())
                                                                                                                                || inventoryNumberHdd.Graphics_cards.Unit.Full_name.ToLower().Contains(inventoryNumberHddFilter.ToLower())
                                                                                                                                || inventoryNumberHdd.Graphics_cards.Unit.Short_name.ToLower().Contains(inventoryNumberHddFilter.ToLower())
                                                                                                                                || inventoryNumberHdd.Graphics_cards.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryNumberHddFilter.ToLower());

        public static bool Search(this Inventory_numbers_ram inventoryRam, string inventoryRamsFilter) => inventoryRam.Ram.Name.ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Unit.Full_name.ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Clock_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Types_memory.Name.ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Unit.Short_name.ToLower().Contains(inventoryRamsFilter.ToLower())
            || inventoryRam.Ram.Manufacturer.Name.ToLower().Contains(inventoryRamsFilter.ToLower());

        public static bool Search(this Inventory_numbers_processors inventoryProcessor, string inventoryProcessorsFilter) => inventoryProcessor.Inventory_number.ToString().ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Unit.Full_name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Base_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Socket.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Amount_cores.ToString().ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Unit.Short_name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
            || inventoryProcessor.Processor.Manufacturer.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower());

        public static bool Search(this Inventory_numbers_power_supplies inventoryPowerSupply, string inventoryPowerSuppliesFilter) => inventoryPowerSupply.Inventory_number.ToString().ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
            || inventoryPowerSupply.Power_supplies.Name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
            || inventoryPowerSupply.Power_supplies.Unit.Full_name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
            || inventoryPowerSupply.Power_supplies.Power.ToString().ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
            || inventoryPowerSupply.Power_supplies.Unit.Short_name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
            || inventoryPowerSupply.Power_supplies.Manufacturer.Name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower());

        #endregion

        #region Other

        public static bool Search(this Socket socket, string socketsFilter) => socket.Name.ToLower().Contains(socketsFilter.ToLower());

        public static bool Search(this Types_hdd typeHdd, string typeHddFilter) => typeHdd.Name.ToLower().Contains(typeHddFilter.ToLower());

        public static bool Search(this Types_memory typeMemory, string typeMemoryFilter) => typeMemory.Name.ToLower().Contains(typeMemoryFilter.ToLower());

        public static bool Search(this Types_ssd typeSsd, string typeSsdFilter) => typeSsd.Name.ToLower().Contains(typeSsdFilter.ToLower());

        public static bool Search(this Unit unit, string unitsFilter) => unit.Full_name.ToLower().Contains(unitsFilter.ToLower()) || unit.Short_name.ToLower().Contains(unitsFilter.ToLower());

        #endregion

        #endregion

        public static bool Search(this Manufacturer manufacturer, string manufacturerFilter) => manufacturer.Name.ToLower().Contains(manufacturerFilter.ToLower());
    }
}
