namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using DevExpress.Mvvm;

    using Inventory.ViewModels.Tables.Employees;

    public partial class Department : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Department()
        {
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
        }
    
        public int Id_department { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Employees_in_departments> Employees_in_departments { get; set; }

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

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool Validation() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        #region Методы взаимодействия с информацией
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
                return Task.FromResult(false);
            }

            findDepartment.Name = post.Name;
            db.SaveChanges();
            return Task.FromResult(true);
        }

        public static Task<bool> DeleteDepartment(Department selectDepartment)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectDepartment.Name}?",
                "Удаление отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return Task.FromResult(false);

            using var db = new InventoryEntities();
            var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == selectDepartment.Id_department);

            if (findDepartment == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
                return Task.FromResult(false);
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
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public static Task<bool> Refresh()
        {
            DepartmentViewModel.Departments.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Departments)
                DepartmentViewModel.Departments.Add(item);

            return Task.FromResult(true);
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