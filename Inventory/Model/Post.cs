namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    public partial class Post : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Post()
        {
            this.Posts_employees = new HashSet<Posts_employees>();
        }

        #region Свойства
        public int Id_post { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Posts_employees> Posts_employees { get; set; }
        #endregion

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = "Поле не должно быть пустым";
                        else if (Name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        #region Методы обработки информации
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
            var findPost = db.Posts.SingleOrDefault(p => p.Id_post == post.Id_post);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении должности",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
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
            var findPost = db.Posts.SingleOrDefault(post => post.Id_post == selectPost.Id_post);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении должности", MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
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
        
        public static Task<bool> Refresh()
        {
            PostViewModel.Posts.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Posts)
                PostViewModel.Posts.Add(item);

            return Task.FromResult(true);
        }
        #endregion

        #region Откат изменений
        private Post _selectPost;

        public void BeginEdit()
        {
            _selectPost = new Post
            {
                Id_post = this.Id_post,
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

            Id_post = _selectPost.Id_post;
            Name = _selectPost.Name;
        }
        #endregion
    }
}