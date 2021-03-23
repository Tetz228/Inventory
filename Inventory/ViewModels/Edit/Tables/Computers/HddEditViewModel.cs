namespace Inventory.ViewModels.Edit.Tables.Computers
{
    using System.Collections.ObjectModel;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class HddEditViewModel : BindableBase
    {
        public HddEditViewModel(Hdd hdd)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            TypesHdds = new ObservableCollection<Types_hdd>(db.Types_hdd);
            Units = new ObservableCollection<Unit>(db.Units);

            Hdd = hdd;
            Hdd.BeginEdit();
        }

        public Hdd Hdd { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_hdd> TypesHdds { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Hdd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Hdd.EndEdit();
            Hdd.EditHdd(Hdd);
            editWindow.Close();
        }, _ => Hdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            Hdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
