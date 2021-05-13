namespace Inventory.Services
{
    using Inventory.Model;
    using System.Globalization;
    using System.Linq;

    public static class SearchFor
    {
        #region Peripherals

        public static bool Search(Inventory_numbers_peripherals inventoryPeripheral, string inventoryPeripheralFilter) => inventoryPeripheral.Inventory_number.ToString().ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Types_peripherals.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Peripheral.Manufacturer.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower())
            || inventoryPeripheral.Statuses_peripherals.Name.ToLower().Contains(inventoryPeripheralFilter.ToLower());

        public static bool Search(Peripheral peripheral, string peripheralFilter) => peripheral.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Types_peripherals.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Manufacturer.Name.ToLower().Contains(peripheralFilter.ToLower());

        public static bool Search(Types_peripherals typePeripheral, string typePeripheralFilter) => typePeripheral.Name.ToLower().Contains(typePeripheralFilter.ToLower());

        public static bool Search(Statuses_peripherals statusPeripheral, string statusPeripheralFilter) => statusPeripheral.Name.ToLower().Contains(statusPeripheralFilter.ToLower());

        #endregion

        #region Employees

        public static bool Search(Department department, string departmentsFilter) => department.Name.ToLower().Contains(departmentsFilter.ToLower());

        public static bool Search(Employee employee, string employeesFilter) => employee.L_name.ToLower().Contains(employeesFilter.ToLower())
            || employee.F_name.ToLower().Contains(employeesFilter.ToLower())
            || employee.Email.ToLower().Contains(employeesFilter.ToLower())
            || employee.Phone_number.ToLower().Contains(employeesFilter.ToLower())
            || employee.Posts_employees.Select(postsEmployees => postsEmployees.Post.Name.ToLower().Contains(employeesFilter.ToLower())).FirstOrDefault()
            || employee.Employees_in_departments.Select(employeesInDepartments => employeesInDepartments.Department.Name.ToLower().Contains(employeesFilter.ToLower())).FirstOrDefault();

        public static bool Search(Post post, string postsFilter) => post.Name.ToLower().Contains(postsFilter.ToLower());

        public static bool Search(User user, string usersFilter) => user.Login.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.L_name.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.F_name.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Employee.Email.ToLower().Contains(usersFilter.ToLower())
                                                                         || user.Role.Name.ToLower().Contains(usersFilter.ToLower());

        #endregion

        #region Computers

        #region Accessories

        public static bool Search(Graphics_cards graphicCard, string graphicCardFilter) => graphicCard.Name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                                             || graphicCard.Unit.Full_name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                                             || (graphicCard.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + graphicCard.Unit.Short_name).ToLower().Contains(graphicCardFilter.ToLower())
                                                                                             || (graphicCard.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + graphicCard.Unit.Full_name).ToLower().Contains(graphicCardFilter.ToLower())
                                                                                             || graphicCard.Manufacturer.Name.ToLower().Contains(graphicCardFilter.ToLower());

        public static bool Search(Hdd hdd, string hddFilter) => hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Unit.Full_name.ToLower().Contains(hddFilter.ToLower())
                                                                  || (hdd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + hdd.Unit.Short_name).ToLower().Contains(hddFilter.ToLower())
                                                                  || (hdd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + hdd.Unit.Full_name).ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Types_hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                  || hdd.Manufacturer.Name.ToLower().Contains(hddFilter.ToLower());

        public static bool Search(Ssd ssd, string ssdFilter) => ssd.Name.ToLower().Contains(ssdFilter.ToLower())
                                                                  || ssd.Unit.Full_name.ToLower().Contains(ssdFilter.ToLower())
                                                                  || (ssd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + ssd.Unit.Short_name).ToLower().Contains(ssdFilter.ToLower())
                                                                  || (ssd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + ssd.Unit.Full_name).ToLower().Contains(ssdFilter.ToLower())
                                                                  || ssd.Types_ssd.Name.ToLower().Contains(ssdFilter.ToLower())
                                                                  || ssd.Manufacturer.Name.ToLower().Contains(ssdFilter.ToLower());

        public static bool Search(Power_supplies powerSupply, string powerSuppliesFilter) => powerSupply.Name.ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                                               || powerSupply.Unit.Full_name.ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                                               || (powerSupply.Power.ToString(CultureInfo.InvariantCulture) + " " + powerSupply.Unit.Short_name).ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                                               || (powerSupply.Power.ToString(CultureInfo.InvariantCulture) + " " + powerSupply.Unit.Full_name).ToLower().Contains(powerSuppliesFilter.ToLower())
                                                                                               || powerSupply.Manufacturer.Name.ToLower().Contains(powerSuppliesFilter.ToLower());

        public static bool Search(Processor processor, string processorsFilter) => processor.Name.ToLower().Contains(processorsFilter.ToLower())
                                                                                     || processor.Unit.Full_name.ToLower().Contains(processorsFilter.ToLower())
                                                                                     || (processor.Base_frequency.ToString(CultureInfo.InvariantCulture) + " " + processor.Unit.Short_name).ToLower().Contains(processorsFilter.ToLower())
                                                                                     || (processor.Base_frequency.ToString(CultureInfo.InvariantCulture) + " " + processor.Unit.Full_name).ToLower().Contains(processorsFilter.ToLower())
                                                                                     || processor.Socket.Name.ToLower().Contains(processorsFilter.ToLower())
                                                                                     || processor.Amount_cores.ToString().ToLower().Contains(processorsFilter.ToLower())
                                                                                     || processor.Manufacturer.Name.ToLower().Contains(processorsFilter.ToLower());

        public static bool Search(Motherboard motherboard, string motherboardFilter) => motherboard.Name.ToLower().Contains(motherboardFilter.ToLower())
                                                                                          || motherboard.Socket.Name.ToLower().Contains(motherboardFilter.ToLower())
                                                                                          || motherboard.Manufacturer.Name.ToLower().Contains(motherboardFilter.ToLower());

        public static bool Search(Ram ram, string ramsFilter) => ram.Name.ToLower().Contains(ramsFilter.ToLower())
                                                                   || ram.Unit.Full_name.ToLower().Contains(ramsFilter.ToLower())
                                                                   || ram.Clock_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(ramsFilter.ToLower())
                                                                   || ram.Types_memory.Name.ToLower().Contains(ramsFilter.ToLower())
                                                                   || (ram.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + ram.Unit.Short_name).ToLower().Contains(ramsFilter.ToLower())
                                                                   || (ram.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + ram.Unit.Full_name).ToLower().Contains(ramsFilter.ToLower())
                                                                   || ram.Manufacturer.Name.ToLower().Contains(ramsFilter.ToLower());

        #endregion

        #region Computers

        public static bool Search(Operating_systems operatingSystem, string operatingSystemsFilter) => operatingSystem.Name.ToLower().Contains(operatingSystemsFilter.ToLower())
            || operatingSystem.System_version.ToLower().Contains(operatingSystemsFilter.ToLower());

        public static bool Search(Statuses_computers statusComputer, string statusComputerFilter) => statusComputer.Name.ToLower().Contains(statusComputerFilter.ToLower());

        public static bool Search(Computer computer, string computerFilter) =>
            computer.Inventory_number.ToString().ToLower().Contains(computerFilter.ToLower())
            || computer.Ip_address.ToLower().Contains(computerFilter.ToLower())
            || computer.Statuses_computers.Name.ToLower().Contains(computerFilter.ToLower())
            || ("Инвент. номер: " + computer.Inventory_numbers_motherboards.Inventory_number).ToLower().Contains(computerFilter.ToLower())
            || (computer.Inventory_numbers_motherboards.Motherboard.Manufacturer.Name + " "
                + computer.Inventory_numbers_motherboards.Motherboard.Name + " "
                + computer.Inventory_numbers_motherboards.Motherboard.Socket.Name).ToLower().Contains(computerFilter.ToLower())
            //|| ("Инвент. номер: " + computer.Inventory_numbers_graphics_cards.Inventory_number).ToLower().Contains(computerFilter.ToLower())
            //|| (computer.Inventory_numbers_graphics_cards.Graphics_cards.Manufacturer.Name + " "
            //    + computer.Inventory_numbers_graphics_cards.Graphics_cards.Name + " "
            //    + computer.Inventory_numbers_graphics_cards.Graphics_cards.Memory_size.ToString(CultureInfo.InvariantCulture) + " "
            //    + computer.Inventory_numbers_graphics_cards.Graphics_cards.Unit.Short_name).ToLower().Contains(computerFilter.ToLower())
            || ("Инвент. номер: " + computer.Inventory_numbers_power_supplies.Inventory_number).ToLower().Contains(computerFilter.ToLower())
            || (computer.Inventory_numbers_power_supplies.Power_supplies.Manufacturer.Name + " "
                + computer.Inventory_numbers_power_supplies.Power_supplies.Name + " "
                + computer.Inventory_numbers_power_supplies.Power_supplies.Power.ToString(CultureInfo.InvariantCulture) + " "
                + computer.Inventory_numbers_power_supplies.Power_supplies.Unit.Short_name).ToLower().Contains(computerFilter.ToLower())
            || computer.Ram_in_computers.Select(inventory => ("Инвент. номер: " + inventory.Inventory_numbers_ram.Inventory_number).ToString().ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Ram_in_computers.Select(ram => (ram.Inventory_numbers_ram.Ram.Types_memory.Name + " "
                + ram.Inventory_numbers_ram.Ram.Manufacturer.Name + " "
                + ram.Inventory_numbers_ram.Ram.Memory_size.ToString(CultureInfo.InvariantCulture) + " "
                + ram.Inventory_numbers_ram.Ram.Unit.Short_name).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Hdd_in_computers.Select(inventory => ("Инвент. номер: " + inventory.Inventory_numbers_hdd.Inventory_number).ToString().ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Hdd_in_computers.Select(hhd => (hhd.Inventory_numbers_hdd.Hdd.Types_hdd.Name + " " 
                + hhd.Inventory_numbers_hdd.Hdd.Manufacturer.Name + " "
                + hhd.Inventory_numbers_hdd.Hdd.Memory_size.ToString(CultureInfo.InvariantCulture) + " "
                + hhd.Inventory_numbers_hdd.Hdd.Unit.Short_name).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Ssd_in_computers.Select(inventory => ("Инвент. номер: " + inventory.Inventory_numbers_ssd.Inventory_number).ToString().ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Ssd_in_computers.Select(ssd => (ssd.Inventory_numbers_ssd.Ssd.Types_ssd.Name + " "
                + ssd.Inventory_numbers_ssd.Ssd.Manufacturer.Name + " "
                + ssd.Inventory_numbers_ssd.Ssd.Memory_size.ToString(CultureInfo.InvariantCulture) + " "
                + ssd.Inventory_numbers_ssd.Ssd.Unit.Short_name).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Operating_systems_in_computers.Select(system => ("Система: " + system.Operating_systems.Name).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Operating_systems_in_computers.Select(system => ("Версия: " + system.Operating_systems.System_version).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Processors_in_computers.Select(inventory => ("Инвент. номер: " + inventory.Inventory_numbers_processors.Inventory_number).ToString().ToLower().Contains(computerFilter.ToLower())).FirstOrDefault()
            || computer.Processors_in_computers.Select(hhd => (hhd.Inventory_numbers_processors.Processor.Name + " "
                + hhd.Inventory_numbers_processors.Processor.Manufacturer.Name).ToLower().Contains(computerFilter.ToLower())).FirstOrDefault();

        #endregion

        #region Inventory numbers

        public static bool Search(Inventory_numbers_motherboards inventoryMotherboard, string inventoryMotherboardFilter) => inventoryMotherboard.Inventory_number.ToString().ToLower().Contains(inventoryMotherboardFilter.ToLower())
                                                                                                                               || inventoryMotherboard.Motherboard.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower())
                                                                                                                               || inventoryMotherboard.Motherboard.Manufacturer.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower())
                                                                                                                               || inventoryMotherboard.Motherboard.Socket.Name.ToLower().Contains(inventoryMotherboardFilter.ToLower());

        public static bool Search(Inventory_numbers_hdd inventoryHdd, string inventoryHddFilter) => inventoryHdd.Inventory_number.ToString().ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || inventoryHdd.Hdd.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || inventoryHdd.Hdd.Types_hdd.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || inventoryHdd.Hdd.Manufacturer.Name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || inventoryHdd.Hdd.Unit.Full_name.ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || (inventoryHdd.Hdd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryHdd.Hdd.Unit.Short_name).ToLower().Contains(inventoryHddFilter.ToLower())
                                                                                                      || (inventoryHdd.Hdd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryHdd.Hdd.Unit.Full_name).ToLower().Contains(inventoryHddFilter.ToLower());

        public static bool Search(Inventory_numbers_ssd inventorySsd, string inventorySsdFilter) => inventorySsd.Inventory_number.ToString().ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || inventorySsd.Ssd.Name.ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || inventorySsd.Ssd.Types_ssd.Name.ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || inventorySsd.Ssd.Manufacturer.Name.ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || inventorySsd.Ssd.Unit.Full_name.ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || (inventorySsd.Ssd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventorySsd.Ssd.Unit.Short_name).ToLower().Contains(inventorySsdFilter.ToLower())
                                                                                                      || (inventorySsd.Ssd.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventorySsd.Ssd.Unit.Full_name).ToLower().Contains(inventorySsdFilter.ToLower());

        public static bool Search(Inventory_numbers_graphics_cards inventoryGraphicsCard, string inventoryGraphicsCardFilter) => inventoryGraphicsCard.Inventory_number.ToString().ToLower().Contains(inventoryGraphicsCardFilter.ToLower())
                                                                                                                                   || inventoryGraphicsCard.Graphics_cards.Name.ToLower().Contains(inventoryGraphicsCardFilter.ToLower())
                                                                                                                                   || inventoryGraphicsCard.Graphics_cards.Manufacturer.Name.ToLower().Contains(inventoryGraphicsCardFilter.ToLower())
                                                                                                                                   || inventoryGraphicsCard.Graphics_cards.Unit.Full_name.ToLower().Contains(inventoryGraphicsCardFilter.ToLower())
                                                                                                                                   || (inventoryGraphicsCard.Graphics_cards.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryGraphicsCard.Graphics_cards.Unit.Short_name).ToLower().Contains(inventoryGraphicsCardFilter.ToLower())
                                                                                                                                   || (inventoryGraphicsCard.Graphics_cards.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryGraphicsCard.Graphics_cards.Unit.Full_name).ToLower().Contains(inventoryGraphicsCardFilter.ToLower());

        public static bool Search(Inventory_numbers_ram inventoryRam, string inventoryRamsFilter) => inventoryRam.Ram.Name.ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || inventoryRam.Ram.Unit.Full_name.ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || inventoryRam.Ram.Clock_frequency.ToString(CultureInfo.InvariantCulture).ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || inventoryRam.Ram.Types_memory.Name.ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || (inventoryRam.Ram.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryRam.Ram.Unit.Short_name).ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || (inventoryRam.Ram.Memory_size.ToString(CultureInfo.InvariantCulture) + " " + inventoryRam.Ram.Unit.Full_name).ToLower().Contains(inventoryRamsFilter.ToLower())
                                                                                                       || inventoryRam.Ram.Manufacturer.Name.ToLower().Contains(inventoryRamsFilter.ToLower());

        public static bool Search(Inventory_numbers_processors inventoryProcessor, string inventoryProcessorsFilter) => inventoryProcessor.Inventory_number.ToString().ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || inventoryProcessor.Processor.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || inventoryProcessor.Processor.Unit.Full_name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || (inventoryProcessor.Processor.Base_frequency.ToString(CultureInfo.InvariantCulture) + " " + inventoryProcessor.Processor.Unit.Short_name).ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || (inventoryProcessor.Processor.Base_frequency.ToString(CultureInfo.InvariantCulture) + " " + inventoryProcessor.Processor.Unit.Full_name).ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || inventoryProcessor.Processor.Socket.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || inventoryProcessor.Processor.Amount_cores.ToString().ToLower().Contains(inventoryProcessorsFilter.ToLower())
                                                                                                                          || inventoryProcessor.Processor.Manufacturer.Name.ToLower().Contains(inventoryProcessorsFilter.ToLower());

        public static bool Search(Inventory_numbers_power_supplies inventoryPowerSupply, string inventoryPowerSuppliesFilter) => inventoryPowerSupply.Inventory_number.ToString().ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || inventoryPowerSupply.Power_supplies.Name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || inventoryPowerSupply.Power_supplies.Unit.Full_name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || inventoryPowerSupply.Power_supplies.Power.ToString().ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || (inventoryPowerSupply.Power_supplies.Power.ToString(CultureInfo.InvariantCulture) + " " + inventoryPowerSupply.Power_supplies.Unit.Short_name).ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || (inventoryPowerSupply.Power_supplies.Power.ToString(CultureInfo.InvariantCulture) + " " + inventoryPowerSupply.Power_supplies.Unit.Full_name).ToLower().Contains(inventoryPowerSuppliesFilter.ToLower())
                                                                                                                                   || inventoryPowerSupply.Power_supplies.Manufacturer.Name.ToLower().Contains(inventoryPowerSuppliesFilter.ToLower());

        #endregion

        #region Other

        public static bool Search(Socket socket, string socketsFilter) => socket.Name.ToLower().Contains(socketsFilter.ToLower());

        public static bool Search(Types_hdd typeHdd, string typeHddFilter) => typeHdd.Name.ToLower().Contains(typeHddFilter.ToLower());

        public static bool Search(Types_memory typeMemory, string typeMemoryFilter) => typeMemory.Name.ToLower().Contains(typeMemoryFilter.ToLower());

        public static bool Search(Types_ssd typeSsd, string typeSsdFilter) => typeSsd.Name.ToLower().Contains(typeSsdFilter.ToLower());

        public static bool Search(Unit unit, string unitsFilter) => unit.Full_name.ToLower().Contains(unitsFilter.ToLower()) || unit.Short_name.ToLower().Contains(unitsFilter.ToLower());

        #endregion

        #endregion

        #region DispensingPeripherals

        public static bool Search(Dispensing_peripherals dispensingPeripherals, string dispensingPeripheralsFilter) =>
            (dispensingPeripherals.Employee.L_name + " " + dispensingPeripherals.Employee.F_name + " " + dispensingPeripherals.Employee.M_name).ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || dispensingPeripherals.Employee.Email.ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || dispensingPeripherals.Employee.Phone_number.ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || (dispensingPeripherals.User.Employee.L_name + " " + dispensingPeripherals.User.Employee.F_name + " " + dispensingPeripherals.User.Employee.M_name).ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || dispensingPeripherals.User.Employee.Email.ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || dispensingPeripherals.User.Employee.Phone_number.ToLower().Contains(dispensingPeripheralsFilter.ToLower())
            || dispensingPeripherals.Date_dispensing.ToString("dd.MM.yyyy").ToLower().Contains(dispensingPeripheralsFilter.ToLower());

        #endregion

        #region DispensingComputers

        public static bool Search(Dispensing_computers dispensingComputers, string dispensingComputersFilter) =>
            (dispensingComputers.Employee.L_name + " " + dispensingComputers.Employee.F_name + " " + dispensingComputers.Employee.M_name).ToLower().Contains(dispensingComputersFilter.ToLower())
            || dispensingComputers.Employee.Email.ToLower().Contains(dispensingComputersFilter.ToLower())
            || dispensingComputers.Employee.Phone_number.ToLower().Contains(dispensingComputersFilter.ToLower())
            || (dispensingComputers.User.Employee.L_name + " " + dispensingComputers.User.Employee.F_name + " " + dispensingComputers.User.Employee.M_name).ToLower().Contains(dispensingComputersFilter.ToLower())
            || dispensingComputers.User.Employee.Email.ToLower().Contains(dispensingComputersFilter.ToLower())
            || dispensingComputers.User.Employee.Phone_number.ToLower().Contains(dispensingComputersFilter.ToLower())
            || dispensingComputers.Date_dispensing.ToString("dd.MM.yyyy").ToLower().Contains(dispensingComputersFilter.ToLower());

        #endregion

        public static bool Search(Manufacturer manufacturer, string manufacturerFilter) => manufacturer.Name.ToLower().Contains(manufacturerFilter.ToLower());
    }
}
