namespace Inventory.View.Edit.Tables.Peripherals
{
    using System.Windows;

    public partial class InventoryNumberPeripheralEditWindow : Window
    {
        public InventoryNumberPeripheralEditWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
