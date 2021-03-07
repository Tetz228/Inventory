namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class PostAddViewModel : BindableBase
    {
        public PostAddViewModel()
        {
            Post = new Post();
        }

        public Post Post { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Add();
            
            addWindow.Close();
        }, _ => Post.Validation());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion

        private async void Add()
        {
            await Post.AddPost(Post.Name);
        }
    }
}