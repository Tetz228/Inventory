namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class PostEditViewModel : BindableBase
    {
        public PostEditViewModel(Post post)
        {
            Post = post;
            Post.BeginEdit();
        }

        public Post Post { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Post.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Post.EndEdit();
            Post.EditPost(Post);
            editWindow.Close();
        }, _ => Post.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Post.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}