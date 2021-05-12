namespace Inventory.View.Edit.Tables.Computers.Computers
{
    using System.Windows;

    public partial class ComputerEditWindow : Window
    {
        public ComputerEditWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
