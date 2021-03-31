namespace Inventory.View.Edit.Tables.Peripherals
{
    using System.Windows;

    public partial class InventoryPeripheralEditWindow : Window
    {
        public InventoryPeripheralEditWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
