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
        public Post Post { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            if(Services.Add(Post))
                PostsViewModel.Posts.Add(Post);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Post.ErrorCollection));
    }
}