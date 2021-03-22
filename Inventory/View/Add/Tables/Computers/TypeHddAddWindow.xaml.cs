using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory.View.Add.Tables.Computers
{
    /// <summary>
    /// Логика взаимодействия для TypeHddAddWindow.xaml
    /// </summary>
    public partial class TypeHddAddWindow : Window
    {
        public TypeHddAddWindow()
        {
            InitializeComponent();
            TextBoxName.Focus();
        }
    }
}
