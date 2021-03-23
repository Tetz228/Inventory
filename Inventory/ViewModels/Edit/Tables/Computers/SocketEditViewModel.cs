﻿namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class SocketEditViewModel : BindableBase
    {
        public SocketEditViewModel(Socket socket)
        {
            Socket = socket;
            Socket.BeginEdit();
        }

        public Socket Socket { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Socket.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Socket.EndEdit();
            Socket.EditSocket(Socket);
            editWindow.Close();
        }, _ => Socket.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Socket.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
