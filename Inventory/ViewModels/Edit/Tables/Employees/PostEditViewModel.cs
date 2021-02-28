namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class PostEditViewModel:BindableBase
    {
        public Post Post { get; set; }

        public ICommand Edit => new DelegateCommand<Window>(editWindow =>
        {
            using var db = new InventoryEntities();
            var findPost = db.Posts.SingleOrDefault(post => post.Id_post == Post.Id_post);

            if (findPost == null)
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении должности", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                findPost.Name = Post.Name;
                db.SaveChanges();
            }
            editWindow.Close();
        }, _ => !string.IsNullOrWhiteSpace(Post.Name));

        public ICommand Cancel => new DelegateCommand<Window>(editWindow => editWindow.Close());
    }
}
