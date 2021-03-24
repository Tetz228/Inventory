using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Recovery__password
{
    using System.ComponentModel;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class NewPasswordViewModel : BindableBase, IDataErrorInfo
    {
        public NewPasswordViewModel(int idUser)
        {
            IdUser = idUser;
        }

        private int IdUser { get; }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Email":
                        
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);
        #endregion
    }
}
