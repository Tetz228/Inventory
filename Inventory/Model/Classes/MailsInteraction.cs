namespace Inventory.Model.Classes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Windows;

    internal class MailsInteraction
    {
        public static (Employee, bool) OnEmailExist(string email)
        {
            using var db = new InventoryEntities();
            var foundEmployee = db.Employees.FirstOrDefault(employee => employee.Email == email);

            if (foundEmployee == null)
            {
                MessageBox.Show("Сотрудник c такой почтой не найден! Проверьте правильность написания почты.", "Ошибка! Сотрудник не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (null, false);
            }

            return (foundEmployee, true);
        }

        public static (int, bool) SendingSecurityCode(string email)
        {
            var random = new Random();
            var fromMailAddress = new MailAddress("itproject719@gmail.com", "ITProject");
            var toMailAddress = new MailAddress(email);
            int code = random.Next(1000, 9999);

            using var mailMessager = new MailMessage(fromMailAddress, toMailAddress)
            {
                Subject = "Восставноление пароля",
                Body = "Ваш код безопасности для восставноления пароля - " + code,
                IsBodyHtml = false
            };

            using var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMailAddress.Address, "%*kHy#l7~x")
            };

            try
            {
                smtp.Send(mailMessager);

                MessageBox.Show("Код безопасности отправлен на почту! Если сообщение с кодом не пришло, то посмотрите в папке спам.", "Код отправлен", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return (code, true);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Код безопасноcти не отправлен.", "Ошибка при отправке кода безопасности.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }
        }

        public static bool IsValidationEmail(string email) => new EmailAddressAttribute().IsValid(email);
    }
}
