namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class SocketAddViewModel: BindableBase
    {
        public SocketAddViewModel()
        {
            Socket = new Socket();
        }

        public Socket Socket { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Socket);
            SocketsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Socket.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
