using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    using System.Windows.Controls;

    using DevExpress.Mvvm;

    using Inventory.View.Pages;

    public class PasswordRecoveryViewModel:BindableBase
    {
        public PasswordRecoveryViewModel()
        {
            CurrentPage = EmailPage = new EmailPage();
        }

        private EmailPage EmailPage { get; set; }

        public Page CurrentPage { get; private set; }
    }
}
