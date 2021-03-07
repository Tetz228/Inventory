namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class PostEditViewModel : BindableBase, IDataErrorInfo
    {
        public PostEditViewModel(Post post)
        {
            Post = post;
            Post.BeginEdit();
        }

        #region Свойства
        private Dictionary<string, string> ErrorCollection { get; set; } = new();

        public string Error { get => null; }

        public string this[string PostName]
        {
            get
            {
                string result = null;

                switch (PostName)
                {
                    case "Post.Name":
                        if (string.IsNullOrWhiteSpace(this.Post.Name))
                            result = "Поле не должно быть пустым";
                        else if (Post.Name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                if (ErrorCollection.ContainsKey(PostName))
                    ErrorCollection[PostName] = result;
                else if (result != null)
                    ErrorCollection.Add(PostName, result);

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }
        public Post Post { get; set; }
        #endregion

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Post.EndEdit();

            Edit();

            editWindow.Close();
        }, _ => Validation());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Post.CancelEdit();

            editWindow.Close();
        });
        #endregion

        private async void Edit() => await Post.EditPost(Post);

        private bool Validation() => ErrorCollection.Any(item => item.Value == null);
    }
}
