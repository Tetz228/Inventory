using System.Linq;

namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add;
    using Inventory.View.Edit;
    using Inventory.ViewModels.Add;
    using Inventory.ViewModels.Edit;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    public class PostViewModel : BindableBase
    {
        public PostViewModel()
        {
            using var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts.ToList());
            PostsCollection = CollectionViewSource.GetDefaultView(Posts);
        }

        #region Свойства
        public ObservableCollection<Post> Posts { get; private set; }

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
        public ICommand DataGridMouseLeftButtonDown => new DelegateCommand(() => SelectPost = null);

        public ICommand AddPost => new DelegateCommand(() =>
        {
            using var db = new InventoryEntities();
            var addPostWindow = new PostAddWindow();
            var addPostViewModel = new PostAddViewModel();

            addPostWindow.DataContext = addPostViewModel;
            addPostWindow.ShowDialog();

            Posts = new ObservableCollection<Post>(db.Posts.ToList());
        });

        public ICommand EditPost => new DelegateCommand<Post>(post =>
        {
            using var db = new InventoryEntities();
            var editPostWindow = new PostEditWindow();
            var editPostViewModel = new PostEditViewModel
            {
                Post = post
            };

            editPostWindow.DataContext = editPostViewModel;
            editPostWindow.ShowDialog();

            Posts = new ObservableCollection<Post>(db.Posts.ToList());
        }, post => post != null);

        public ICommand DeletePost => new DelegateCommand<Post>((allottedPost) =>
        {
            using var db = new InventoryEntities();
            var findPost = db.Posts.SingleOrDefault(post => post.Id_post == allottedPost.Id_post);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении должности", MessageBoxButton.OK, MessageBoxImage.Error);
                Posts = new ObservableCollection<Post>(db.Posts.ToList());
                return;
            }

            db.Posts.Remove(findPost);
            db.SaveChanges();

            Posts.Remove(allottedPost);
        }, (allottedPost) => allottedPost != null);
        #endregion
    }
}
