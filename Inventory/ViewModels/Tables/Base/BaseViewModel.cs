namespace Inventory.ViewModels.Tables.Base
{
    using System;

    using DevExpress.Mvvm;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Services;

    public abstract class BaseViewModel<TClass> : BindableBase
    {
        private readonly string _nameTemplate;
        private readonly string _namedAreaName;
        private readonly Action _refreshCollectionAction;

        protected BaseViewModel(ObservableCollection<TClass> observableCollection) => CollectionView = CollectionViewSource.GetDefaultView(observableCollection);

        protected BaseViewModel(ObservableCollection<TClass> observableCollection, Action refreshCollectionAction)
        {
            CollectionView = CollectionViewSource.GetDefaultView(observableCollection);
            _refreshCollectionAction = refreshCollectionAction;
        }

        protected BaseViewModel(ObservableCollection<TClass> observableCollection, Action refreshCollectionAction, string nameTemplate, string namedAreaName)
        {
            CollectionView = CollectionViewSource.GetDefaultView(observableCollection);
            _nameTemplate = nameTemplate;
            _namedAreaName = namedAreaName;
            _refreshCollectionAction = refreshCollectionAction;
        }

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

        public ICommand ExportExcelCommand => new DelegateCommand<ICollectionView>(collectionView => collectionView.ExportExcel(_nameTemplate, _namedAreaName));

        public ICommand RefreshCollectionCommand => new DelegateCommand(_refreshCollectionAction);

        #region События

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectObject = default;

        public abstract void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args);

        #endregion
    }
}
