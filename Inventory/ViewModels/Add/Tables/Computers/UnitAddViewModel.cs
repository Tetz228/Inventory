namespace Inventory.ViewModels.Add.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class UnitAddViewModel : BindableBase
    {
        public UnitAddViewModel()
        {
            Unit = new Unit();
        }

        public Unit Unit { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Unit.AddUnit(Unit.Full_name, Unit.Short_name);
            addWindow.Close();
        }, _ => Unit.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
