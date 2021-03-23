namespace Inventory.ViewModels.Add.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class TypeSddAddViewModel : BindableBase
    {
        public TypeSddAddViewModel()
        {
            TypeSsd = new Types_ssd();
        }

        public Types_ssd TypeSsd { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Types_ssd.AddTypeSsd(TypeSsd.Name);
            addWindow.Close();
        }, _ => TypeSsd.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
