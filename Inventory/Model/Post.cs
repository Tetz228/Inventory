namespace Inventory.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;

    public partial class Post : BindableBase, IEditableObject
    {
        public Post()
        {
            this.PostsEmployees = new HashSet<Posts_employees>();
        }

        #region Свойства
        public int IdPost { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Posts_employees> PostsEmployees { get; set; }
        #endregion

        #region Методы взаимодействия с информацией
        public static Task<bool> AddPost(string name)
        {
            using var db = new InventoryEntities();

            var post = new Post
            {
                Name = name
            };

            db.Posts.Add(post);
            db.SaveChanges();

            PostViewModel.Posts.Add(post);

            return Task.FromResult(true);
        }

        public static Task<bool> EditPost(Post post)
        {
            using var db = new InventoryEntities();
            var findPost = db.Posts.SingleOrDefault(p => p.IdPost == post.IdPost);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении должности",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return Task.FromResult(false);
            }

            findPost.Name = post.Name;
            db.SaveChanges();
            return Task.FromResult(true);
        }

        public static Task<bool> DeletePost(Post selectPost)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectPost.Name}?",
                "Удаление должности", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return Task.FromResult(false);

            using var db = new InventoryEntities();
            var findPost = db.Posts.SingleOrDefault(post => post.IdPost == selectPost.IdPost);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении должности", MessageBoxButton.OK, MessageBoxImage.Error);
                return Task.FromResult(false); 
            }

            try
            {
                db.Posts.Remove(findPost);
                db.SaveChanges();

                PostViewModel.Posts.Remove(selectPost);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить должность, так как она связана с другими сущностями!",
                    "Ошибка при удалении должности", MessageBoxButton.OK, MessageBoxImage.Error);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        #endregion

        private Post _selectPost;

        public void BeginEdit()
        {
            _selectPost = new Post
            {
                IdPost = this.IdPost,
                Name = this.Name,
            };
        }

        public void EndEdit()
        {
            _selectPost = null;
        }

        public void CancelEdit()
        {
            if (_selectPost == null) 
                return;

            IdPost = _selectPost.IdPost;
            Name = _selectPost.Name;
        }
    }
}
