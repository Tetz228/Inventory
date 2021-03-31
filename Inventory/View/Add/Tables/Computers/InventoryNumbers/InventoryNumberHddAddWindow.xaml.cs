namespace Inventory.View.Add.Tables.Computers.InventoryNumbers
{
    using System.Windows;

    /// <summary>
    /// Логика взаимодействия для InventoryHddEditWindow.xaml
    /// </summary>
    public partial class InventoryNumberHddAddWindow : Window
    {
        public InventoryNumberHddAddWindow()
        {
            InitializeComponent();
            TextBoxInventoryNumber.Focus();
        }
    }
}
