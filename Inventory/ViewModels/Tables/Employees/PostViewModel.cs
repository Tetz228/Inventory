namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Input;

    public class PostViewModel : BindableBase
    {
        public PostViewModel()
        {
            using var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts);
            PostsCollection = CollectionViewSource.GetDefaultView(Posts);
        }

        #region Свойства
        public static ObservableCollection<Post> Posts { get; set; } = new();

        private ICollectionView PostsCollection { get; }

        public Post SelectPost { get; set; }

        private string _searchPost;

        public string SearchPost
        {
            get => _searchPost;
            set
            {
                _searchPost = value;
                PostsCollection.Filter = obj =>
                {
                    if (obj is Post post)
                        return post.Name.ToLower().Contains(SearchPost.ToLower());

                    return false;
                };
                PostsCollection.Refresh();
            }
        }
        #endregion

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

        public ICommand DeletePost => new DelegateCommand<Post>(Delete, selectPost => selectPost != null);

        public ICommand RefreshList => new DelegateCommand(Refresh);
        #endregion

        private async void Refresh() => await Post.Refresh();

        private async void Delete(Post post) => await Post.DeletePost(post);
    }
}
