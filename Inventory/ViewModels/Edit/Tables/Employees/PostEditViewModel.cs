namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class PostEditViewModel : BindableBase
    {
        public PostEditViewModel(Post post)
        {
            Post = post;
            Post.BeginEdit();
        }

        #region Свойства
        public Post Post { get; }
        #endregion

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Post.EndEdit();
            Edit();
            editWindow.Close();
        }, _ => Post.Validation());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Post.CancelEdit();
            editWindow.Close();
        });
        #endregion

        private async void Edit() => await Post.EditPost(Post);
    }
}