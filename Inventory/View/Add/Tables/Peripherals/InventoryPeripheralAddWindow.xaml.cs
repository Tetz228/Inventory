namespace Inventory.View.Add.Tables.Peripherals
{
    using System.Windows;

    public partial class InventoryPeripheralAddWindow : Window
    {
        public InventoryPeripheralAddWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
