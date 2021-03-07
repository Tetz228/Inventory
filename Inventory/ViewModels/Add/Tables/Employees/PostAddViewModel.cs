namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class PostAddViewModel : BindableBase, IDataErrorInfo
    {
        #region Свойства
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "PostName":
                        if (string.IsNullOrWhiteSpace(PostName))
                            result = "Поле не должно быть пустым";
                        else if (PostName.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public string PostName { get; set; }
        #endregion

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Add();
            
            addWindow.Close();
        }, _ => Validation());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion

        #region Методы
        private bool Validation() => ErrorCollection.Any(item => item.Value == null);

        private async void Add()
        {
            await Post.AddPost(PostName);
        }
        #endregion
    }
}