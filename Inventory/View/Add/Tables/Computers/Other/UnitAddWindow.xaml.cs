namespace Inventory.View.Add.Tables.Computers
{
    using System.Windows;

    /// <summary>
    /// Логика взаимодействия для UnitAddWindow.xaml
    /// </summary>
    public partial class UnitAddWindow : Window
    {
        public UnitAddWindow()
        {
            InitializeComponent();
            TextBoxFullName.Focus();
        }
    }
}
