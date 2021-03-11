namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using System.ComponentModel.DataAnnotations;

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