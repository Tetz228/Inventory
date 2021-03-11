namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System;
    using System.Runtime.InteropServices;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Security;
    using System.Security.Cryptography;
    using System.Windows;
    using System.Windows.Input;

    using BCrypt.Net;

    public class UserAddViewModel : BindableBase
    {
        public UserAddViewModel()
        {
            User = new User();
        }

        public User User { get; }

        public SecureString SecurePassword { private get; set; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            BAD_ConvertToUnsecureString(SecurePassword);
            //User.AddUser(User);



            addWindow.Close();

        }, _ => true);

        public static string BAD_ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);

            var password = Marshal.PtrToStringUni(unmanagedString);

            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);

            string salt = BCrypt.GenerateSalt(7);
            var h =BCrypt.HashPassword(password, salt);
            

            return password;
        }

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
