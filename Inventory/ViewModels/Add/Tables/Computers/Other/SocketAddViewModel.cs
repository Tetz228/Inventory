namespace Inventory.ViewModels.Add.Tables.Computers
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

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
            Socket.AddSocket(Socket.Name);
            addWindow.Close();
        }, _ => Socket.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
