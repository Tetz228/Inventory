﻿namespace Inventory.Services
{
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;

    using BCrypt.Net;

    using Inventory.Model;
    using Inventory.View;
    using Inventory.View.Pages.RecoveryPassword;
    using Inventory.ViewModels.RecoveryPassword;

    public static class UsersInteraction
    {
        public static void ChangePassword(User user)
        {
            (string salt, string hash) = GenerateSaltAndHashingPassword(user.Password);

            user.Salt = salt;
            user.Password = hash;

            using var db = new InventoryEntities();

            var foundUser = db.Users.FirstOrDefault(u => u.Id_user == user.Id_user);

            if (foundUser == null)
            {
                MessageBox.Show("Объект не найден в базе данных! Пароль не был изменен.", "Ошибка при изменение пароля", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foundUser.Password = user.Password;
            foundUser.Salt = user.Salt;

            db.SaveChanges();

            MessageBox.Show("Пароль успешно изменен!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static (string salt, string hash) GenerateSaltAndHashingPassword(string password)
        {
            var salt = BCrypt.GenerateSalt(4);
            var hash = BCrypt.HashPassword(password, salt);

            return (salt, hash);
        }

        public static User OnUserExist(string login, string password)
        {
            using var db = new InventoryEntities();
            var foundUser = db.Users.Include(role => role.Role).FirstOrDefault(user => user.Login == login);

            if (foundUser == null)
            {
                MessageBox.Show("Пользователь не найден! Проверьте правильность написания логина.", "Ошибка! Пользователь не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return null;
            }

            if (foundUser.Password == " ")
            {
                OnWindowChangePassword(foundUser.Id_user);

                return null;
            }
            
            if (BCrypt.Verify(password, foundUser.Password) == false)
            {
                MessageBox.Show("Неверный пароль! Проверьте правильность написания пароля.", "Ошибка! Неверный пароль.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return null;
            }

            return foundUser;
        }

        public static (int, bool) OnUserExist(Employee employee)
        {
            using var db = new InventoryEntities();
            var foundUser = db.Users.FirstOrDefault(user => user.Fk_employee == employee.Id_employee);

            if (foundUser == null)
            {
                MessageBox.Show("Пользователь c такой почтой не найден!", "Ошибка! Пользователь не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }

            return (foundUser.Id_user, true);
        }

        public static void OnWindowChangePassword(int idUser)
        {
            var passwordRecoveryWindow = new PasswordRecoveryWindow();
            var newPassword = new NewPasswordPage();
            var newPasswordViewModel = new NewPasswordViewModel(idUser);
            newPassword.DataContext = newPasswordViewModel;

            PasswordRecoveryViewModel.PageNavigation.Navigate(newPassword);
            passwordRecoveryWindow.ShowDialog();
        }
    }
}
