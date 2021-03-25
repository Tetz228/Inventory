namespace Inventory.View.Add.Tables.Computers.Other
{
    using System.Windows;

    /// <summary>
    /// Логика взаимодействия для SocketAddWindow.xaml
    /// </summary>
    public partial class SocketAddWindow : Window
    {
        public SocketAddWindow()
        {
            InitializeComponent();
            TextBoxName.Focus();
        }
    }
}
