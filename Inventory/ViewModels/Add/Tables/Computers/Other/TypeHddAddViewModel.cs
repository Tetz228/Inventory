namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class TypeHddAddViewModel : BindableBase
    {
        public TypeHddAddViewModel()
        {
            TypeHdd = new Types_hdd();
        }

        public Types_hdd TypeHdd { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypeHdd);
            Types_hdd.RefreshCollection();
            addWindow.Close();
        }, _ => TypeHdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
