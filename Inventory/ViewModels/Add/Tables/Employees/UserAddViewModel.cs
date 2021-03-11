using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class UserAddViewModel : BindableBase
    {
        public UserAddViewModel()
        {
            User = new User();
        }

        public User User { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            User.AddUser(User);
            addWindow.Close();
        }, _ => User.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
