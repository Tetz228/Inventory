namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class HddEditViewModel : BindableBase
    {
        public HddEditViewModel(Hdd hdd)
        {
            Hdd = hdd;
            Hdd.BeginEdit();
        }

        public Hdd Hdd { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Hdd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Hdd.EndEdit();
            Hdd.EditHdd(Hdd);
            editWindow.Close();
        }, _ => Hdd.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Hdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
