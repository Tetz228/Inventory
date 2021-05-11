namespace Inventory.ViewModels.Tables.Base
{
    using DevExpress.Mvvm;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;

    public abstract class BaseViewModel<TClass> : BindableBase
    {
        protected BaseViewModel(ObservableCollection<TClass> observableCollection) => CollectionView = CollectionViewSource.GetDefaultView(observableCollection);

        #region Свойства

        protected ICollectionView CollectionView { get; }

        protected ListSortDirection SortDirection { get; set; }

        public TClass SelectObject { get; set; }

        #endregion

        #region События

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectObject = default;

        public abstract void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args);

        #endregion
    }
}
