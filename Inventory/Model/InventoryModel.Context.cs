﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InventoryEntities : DbContext
    {
        public InventoryEntities()
            : base("name=InventoryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Dispensing_computers> Dispensing_computers { get; set; }
        public virtual DbSet<Dispensing_peripherals> Dispensing_peripherals { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Employees_in_departments> Employees_in_departments { get; set; }
        public virtual DbSet<Graphics_cards> Graphics_cards { get; set; }
        public virtual DbSet<Hdd> Hdds { get; set; }
        public virtual DbSet<Hdd_in_computers> Hdd_in_computers { get; set; }
        public virtual DbSet<History_dispensed_computers> History_dispensed_computers { get; set; }
        public virtual DbSet<History_dispensed_peripherals> History_dispensed_peripherals { get; set; }
        public virtual DbSet<Inventory_numbers_graphics_cards> Inventory_numbers_graphics_cards { get; set; }
        public virtual DbSet<Inventory_numbers_hdd> Inventory_numbers_hdd { get; set; }
        public virtual DbSet<Inventory_numbers_motherboards> Inventory_numbers_motherboards { get; set; }
        public virtual DbSet<Inventory_numbers_peripherals> Inventory_numbers_peripherals { get; set; }
        public virtual DbSet<Inventory_numbers_power_supplies> Inventory_numbers_power_supplies { get; set; }
        public virtual DbSet<Inventory_numbers_processors> Inventory_numbers_processors { get; set; }
        public virtual DbSet<Inventory_numbers_ram> Inventory_numbers_ram { get; set; }
        public virtual DbSet<Inventory_numbers_ssd> Inventory_numbers_ssd { get; set; }
        public virtual DbSet<List_dispensed_computers> List_dispensed_computers { get; set; }
        public virtual DbSet<List_dispensed_peripherals> List_dispensed_peripherals { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Motherboard> Motherboards { get; set; }
        public virtual DbSet<Operating_systems> Operating_systems { get; set; }
        public virtual DbSet<Operating_systems_in_computers> Operating_systems_in_computers { get; set; }
        public virtual DbSet<Peripheral> Peripherals { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Posts_employees> Posts_employees { get; set; }
        public virtual DbSet<Power_supplies> Power_supplies { get; set; }
        public virtual DbSet<Processor> Processors { get; set; }
        public virtual DbSet<Processors_in_computers> Processors_in_computers { get; set; }
        public virtual DbSet<Ram> Rams { get; set; }
        public virtual DbSet<Ram_in_computers> Ram_in_computers { get; set; }
        public virtual DbSet<Socket> Sockets { get; set; }
        public virtual DbSet<Ssd> Ssds { get; set; }
        public virtual DbSet<Ssd_in_computers> Ssd_in_computers { get; set; }
        public virtual DbSet<Statuses_computers> Statuses_computers { get; set; }
        public virtual DbSet<Statuses_peripherals> Statuses_peripherals { get; set; }
        public virtual DbSet<Types_hdd> Types_hdd { get; set; }
        public virtual DbSet<Types_memory> Types_memory { get; set; }
        public virtual DbSet<Types_peripherals> Types_peripherals { get; set; }
        public virtual DbSet<Types_ssd> Types_ssd { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
    }
}
