namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Department : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
        }

        public int Id_department { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        public static bool SearchFor(Department department, string departmentsFilter) => department.Name.ToLower().Contains(departmentsFilter.ToLower());

        public static void RefreshCollection()
        {
            DepartmentsViewModel.Departments.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Departments)
                DepartmentsViewModel.Departments.Add(item);
        }

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
