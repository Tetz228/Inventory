namespace Inventory.ViewModels.Add.Tables.Computers.Other
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Other;

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
            TypesHddsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypeHdd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
