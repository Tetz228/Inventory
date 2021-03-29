namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;

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
            Services.Add(Post);
            PostsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Post.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}