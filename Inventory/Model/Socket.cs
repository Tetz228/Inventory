namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Socket : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Socket()
        {
            this.Motherboards = new HashSet<Motherboard>();
            this.Processors = new HashSet<Processor>();
        }

        public int Id_socket { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Motherboard> Motherboards { get; set; }
        public virtual ICollection<Processor> Processors { get; set; }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = "Поле не должно быть пустым";
                        else if (Name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        #region Метод поиска
        public static bool Search(Socket socket, string socketsFilter) => socket.Name.ToLower().Contains(socketsFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddSocket(string name)
        {
            using var db = new InventoryEntities();

            var post = new Socket
            {
                Name = name
            };

            db.Sockets.Add(post);
            db.SaveChanges();

            SocketsViewModel.Sockets.Add(post);
        }

        public static void EditSocket(Socket socket)
        {
            using var db = new InventoryEntities();
            var findPost = db.Sockets.FirstOrDefault(s => s.Id_socket == socket.Id_socket);

            if (findPost == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении сокета",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findPost.Name = socket.Name;
            db.SaveChanges();
        }

        public static void DeleteSocket(Socket selectSocket)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectSocket.Name}?",
                "Удаление сокета", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findSocket = db.Sockets.FirstOrDefault(socket => socket.Id_socket == selectSocket.Id_socket);

            if (findSocket == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении сокета", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Sockets.Remove(findSocket);
                db.SaveChanges();

                SocketsViewModel.Sockets.Remove(selectSocket);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить сокет, так как он связан с другими сущностями!",
                    "Ошибка при удалении сокета", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            SocketsViewModel.Sockets.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Sockets)
                SocketsViewModel.Sockets.Add(item);
        }
        #endregion

        #region Откат изменений
        private Socket _selectSocket;

        public void BeginEdit()
        {
            _selectSocket = new Socket
            {
                Id_socket = this.Id_socket,
                Name = this.Name,
            };
        }

        public void EndEdit()
        {
            _selectSocket = null;
        }

        public void CancelEdit()
        {
            if (_selectSocket == null)
                return;

            Id_socket = _selectSocket.Id_socket;
            Name = _selectSocket.Name;
        }
        #endregion
    }
}
