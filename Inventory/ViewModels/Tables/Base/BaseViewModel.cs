namespace Inventory.ViewModels.Tables.Base
{
    using DevExpress.Mvvm;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using Inventory.Services;

    public abstract class BaseViewModel<TClass> : BindableBase
    {
        protected BaseViewModel(ObservableCollection<TClass> observableCollection) => CollectionView = CollectionViewSource.GetDefaultView(observableCollection);

        #region Свойства

        public ICollectionView CollectionView { get; }

        protected ListSortDirection SortDirection { get; set; }

        public TClass SelectObject { get; set; }

        private string _filter = string.Empty;

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                CollectionView.Filter = obj =>
                {
                    if (obj is TClass @class)
                    {
                        dynamic dynamicClass = @class;
                        return SearchFor.Search(dynamicClass, Filter);
                    }

                    return false;
                };
                CollectionView.Refresh();
            }
        }

        #endregion

        #region События

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectObject = default;

        public abstract void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args);

        #endregion
    }
}
