namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class PostEditViewModel : BindableBase, IEditableObject
    {
        public PostEditViewModel(Post post)
        {
            Post = post;
            BeginEdit();
        }

        public Post Post { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Post.Id_post, Post);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Post.ErrorCollection));


        #region Откат изменений
        private Post _selectPost;

        public void BeginEdit()
        {
            _selectPost = new Post
            {
                Id_post = Post.Id_post,
                Name = Post.Name,
            };
        }

        public void EndEdit() => _selectPost = null;

        public void CancelEdit()
        {
            if (_selectPost == null)
                return;

            Post.Id_post = _selectPost.Id_post;
            Post.Name = _selectPost.Name;
        }
        #endregion
    }
}