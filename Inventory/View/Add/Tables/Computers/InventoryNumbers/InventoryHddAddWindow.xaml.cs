namespace Inventory.View.Add.Tables.Computers.InventoryNumbers
{
    using System.Windows;

    /// <summary>
    /// Логика взаимодействия для InventoryHddEditWindow.xaml
    /// </summary>
    public partial class InventoryHddAddWindow : Window
    {
        public InventoryHddAddWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
