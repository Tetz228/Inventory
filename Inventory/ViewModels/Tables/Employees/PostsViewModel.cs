namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Services;

    public class PostsViewModel : BindableBase
    {
        public PostsViewModel()
        {
            using var db = new InventoryEntities();

            Posts = new ObservableCollection<Post>(db.Posts);
            Posts.Sort(department => department.Name, SortDirection = ListSortDirection.Ascending);
            PostsCollection = CollectionViewSource.GetDefaultView(Posts);
        }

        #region Свойства
        private ICollectionView PostsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Post> Posts { get; set; }

        public Post SelectPost { get; set; }

        private string _postsFilter = string.Empty;

        public string PostsFilter
        {
            get => _postsFilter;
            set
            {
                _postsFilter = value;
                PostsCollection.Filter = obj =>
                {
                    if (obj is Post post)
                        return post.Search(PostsFilter);

                    return false;
                };
                PostsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            Posts.Sort(post => post.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectPost = null;
        #endregion

        #region Команды
        public ICommand AddPostCommand => new DelegateCommand(() =>
        {
            var addPostWindow = new PostAddWindow();
            addPostWindow.ShowDialog();
        });

        public ICommand EditPostCommand => new DelegateCommand<Post>(post =>
        {
            var editPostWindow = new PostEditWindow();
            var editPostViewModel = new PostEditViewModel(post);
            editPostWindow.DataContext = editPostViewModel;
            editPostWindow.Closing += editPostViewModel.OnWindowClosing;
            editPostWindow.ShowDialog();

        }, post => post != null);

        public ICommand DeletePostCommand => new DelegateCommand<Post>(selectPost =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectPost.Name}?", "Удаление должности", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Post>(selectPost.Id_post);
            RefreshCollection();
        }, selectPost => selectPost != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Posts.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Posts)
                Posts.Add(item);
        }
    }
}
