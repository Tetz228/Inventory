namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    public partial class Department : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Department()
        {
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
        }

        #region Свойства
        public int Id_department { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employees_in_departments> Employees_in_departments { get; set; }
        #endregion

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

        #region Методы обработки информации
        public static Task<bool> AddDepartment(string name)
        {
            using var db = new InventoryEntities();

            var department = new Department()
            {
                Name = name
            };

            db.Departments.Add(department);
            db.SaveChanges();

            DepartmentViewModel.Departments.Add(department);

            return Task.FromResult(true);
        }

        public static Task<bool> EditDepartment(Department post)
        {
            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == post.Id_department);

            if (findDepartment == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении отдела",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
                return Task.FromResult(false);
            }

            findDepartment.Name = post.Name;
            db.SaveChanges();
            return Task.FromResult(true);
        }

        public static void DeleteDepartment(Department selectDepartment)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectDepartment.Name}?",
                "Удаление отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == selectDepartment.Id_department);

            if (findDepartment == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
                return;
            }

            try
            {
                db.Departments.Remove(findDepartment);
                db.SaveChanges();

                DepartmentViewModel.Departments.Remove(selectDepartment);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить отдел, так как он связан с другими сущностями!",
                    "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void Refresh()
        {
            DepartmentViewModel.Departments.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Departments)
                DepartmentViewModel.Departments.Add(item);
        }
        #endregion

        #region Откат изменений
        private Department _selectDepartment;

        public void BeginEdit()
        {
            _selectDepartment = new Department()
            {
                Id_department = this.Id_department,
                Name = this.Name,
            };
        }

        public void EndEdit()
        {
            _selectDepartment = null;
        }

        public void CancelEdit()
        {
            if (_selectDepartment == null)
                return;

            Id_department = _selectDepartment.Id_department;
            Name = _selectDepartment.Name;
        }
        #endregion
    }
}