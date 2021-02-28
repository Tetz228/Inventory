namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class PostAddViewModel : BindableBase
    {
        public string PostName { get; set; }

        public ICommand Add => new DelegateCommand<Window>(addWindow =>
        {
            using var db = new InventoryEntities();

            var post = new Post
            {
                Name = PostName
            };

            db.Posts.Add(post);
            db.SaveChanges();

            addWindow.Close();
        }, _ => !string.IsNullOrWhiteSpace(PostName));

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
    }
}