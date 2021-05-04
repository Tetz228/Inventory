namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Windows;
    using System.Windows.Input;

    public class SocketAddViewModel : BindableBase
    {
        public Socket Socket { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Socket);
            SocketsViewModel.Sockets.Add(Socket);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Socket.ErrorCollection));
    }
}
