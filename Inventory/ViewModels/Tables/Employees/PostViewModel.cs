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

    public class PostViewModel : BindableBase
    {
        public PostViewModel()
        {
            using var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts);
            PostsCollection = CollectionViewSource.GetDefaultView(Posts);
            PostsCollection.SortDescriptions.Add(new SortDescription(nameof(Post.Name), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Post> Posts { get; set; }

        public ICollectionView PostsCollection { get; }

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
                        return Post.Search(post, PostsFilter);

                    return false;
                };
                PostsCollection.Refresh();
            }
        }
        #endregion

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                    {
                        if (PostsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            PostsCollection.SortDescriptions.Clear();
                            PostsCollection.SortDescriptions.Add(new SortDescription(nameof(Post.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            PostsCollection.SortDescriptions.Clear();
                            PostsCollection.SortDescriptions.Add(new SortDescription(nameof(Post.Name), ListSortDirection.Ascending));
                        }

                        PostsCollection.Refresh();

                        break;
                    }
            }
        }

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectPost = null);

        public ICommand AddPost => new DelegateCommand(() =>
        {
            var addPostWindow = new PostAddWindow();

            addPostWindow.ShowDialog();
        });

        public ICommand EditPost => new DelegateCommand<Post>(post =>
        {
            var editPostWindow = new PostEditWindow();
            var editPostViewModel = new PostEditViewModel(post);

            editPostWindow.DataContext = editPostViewModel;
            editPostWindow.ShowDialog();

        }, post => post != null);

        public ICommand DeletePost => new DelegateCommand<Post>(Post.DeletePost, selectPost => selectPost != null);

        public ICommand RefreshList => new DelegateCommand(Post.Refresh);
        #endregion
    }
}
