namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class UserEditViewModel : BindableBase, IEditableObject
    {
        public UserEditViewModel(User user)
        {
            User = user;
            using var db = new InventoryEntities();
            Roles = new ObservableCollection<Role>(db.Roles);
            BeginEdit();
        }

        #region Свойства
        public User User { get; }

        public ObservableCollection<Role> Roles { get; }
        #endregion

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды

        public ICommand PasswordResetCommand => new DelegateCommand(() =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите сбросить пароль пользователю?", "Сброс пароля", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            User.Password = " ";

            MessageBox.Show("Успех! Пароль успешно сброшен. Для того чтобы применить изменения нажмите на кнопку \"Изменить пользователя\"", "Пароль сброшен", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }, true);

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(User.Id_user, User);
            UsersViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => true);

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private User _selectPost;

        public void BeginEdit()
        {
            _selectPost = new User()
            {
                Fk_role = User.Fk_role,
                Password = User.Password
            };
        }

        public void EndEdit()
        {
            _selectPost = null;
        }

        public void CancelEdit()
        {
            if (_selectPost == null)
                return;

            User.Fk_role = _selectPost.Fk_role;
            User.Password = _selectPost.Password;
        }
        #endregion
    }
}
