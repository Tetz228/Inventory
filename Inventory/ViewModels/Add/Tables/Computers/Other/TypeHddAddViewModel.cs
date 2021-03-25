namespace Inventory.ViewModels.Add.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

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
            Types_hdd.AddTypeHdd(TypeHdd.Name);
            addWindow.Close();
        }, _ => TypeHdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
