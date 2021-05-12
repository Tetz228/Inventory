namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class PostsViewModel : BaseViewModel<Post>
    {
        public PostsViewModel() : base(Posts) => RefreshCollection();

        public static ObservableCollection<Post> Posts { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Наименование":
                        {
                            Posts.Sort(post => post.Name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddPostCommand => new DelegateCommand(() =>
        {
            var addPostWindow = new PostAddWindow();
            addPostWindow.ShowDialog();
        });

        public ICommand EditPostCommand => new DelegateCommand<Post>(post =>
        {
            var editPostWindow = new PostEditWindow();
            var editPostViewModel = new PostEditViewModel(post);
            editPostWindow.DataContext = editPostViewModel;
            editPostWindow.Closing += editPostViewModel.OnWindowClosing;
            editPostWindow.ShowDialog();
        }, post => post != null);

        public ICommand DeletePostCommand => new DelegateCommand<Post>(selectPost =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить должность:\nнаименование - {selectPost.Name}?", "Удаление должности", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Post>(selectPost.Id_post))
                Posts.Remove(selectPost);
        }, selectPost => selectPost != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
   
        private static void RefreshCollection()
        {
            Posts.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Posts.AsNoTracking())
            {
                Posts.Add(item);
            }
            Posts.Sort(post => post.Name);
        }
    }
}
