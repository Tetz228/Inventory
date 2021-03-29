namespace Inventory.ViewModels.Edit.Tables.Computers.Other
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

    public class SocketEditViewModel : BindableBase, IEditableObject
    {
        public SocketEditViewModel(Socket socket)
        {
            Socket = socket;
            BeginEdit();
        }

        public Socket Socket { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
           EndEdit();
            Services.Edit(Socket.Id_socket, Socket);
            SocketsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Socket.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Socket _selectSocket;

        public void BeginEdit()
        {
            _selectSocket = new Socket
            {
                Id_socket = Socket.Id_socket,
                Name = Socket.Name,
            };
        }

        public void EndEdit()
        {
            _selectSocket = null;
        }

        public void CancelEdit()
        {
            if (_selectSocket == null)
                return;

            Socket.Id_socket = _selectSocket.Id_socket;
            Socket.Name = _selectSocket.Name;
        }
        #endregion
    }
}
