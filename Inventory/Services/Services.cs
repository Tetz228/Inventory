namespace Inventory.Services
{
    using Inventory.Model;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    public static class Services
    {
        public static void Sort<TClassSource, TProperty>(this ObservableCollection<TClassSource> observableCollection, Func<TClassSource, TProperty> property, ListSortDirection sortDirection)
        {
            var sortCollection = sortDirection == ListSortDirection.Ascending
                ? observableCollection.OrderBy(property).ToList()
                : observableCollection.OrderByDescending(property).ToList();

            observableCollection.Clear();

            foreach (var item in sortCollection)
                observableCollection.Add(item);
        }

        public static bool IsValidationProperties<TClass>(TClass dictionary) where TClass : Dictionary<string, string> => dictionary.Count == 0
                                                                             || dictionary.All(item => item.Value == null);

        /// <summary>
        /// Поиск инвентарного номера с помощью дерева выражений
        /// </summary>
        /// <typeparam name="TClass">Класс, который содержит инвентарный номер</typeparam>
        /// <param name="inventoryNumber">Инвентарный номер</param>
        /// <param name="selectInventoryNumber">Инвентарный номер, который записан в переменную для отката изменений</param>
        /// <returns>Возвращает null, когда инвентарный номер не найден в базе данных, иначе "Номер должен быть уникальным" </returns>
        public static string IsSavingDocumentExcel()
        {
            var saveExcelDoc = new SaveFileDialog
            {
                Filter = "Excel документ|*.xlsx",
                Title = "Сохранение данных в Excel"
            };
            saveExcelDoc.ShowDialog();

            return saveExcelDoc.FileName != "" ? saveExcelDoc.FileName : null;
        }

        /// <summary>
        /// Проверка на уникальность определенного свойства
        /// </summary>
        /// <typeparam name="TClass">Класс, который содержит свойство</typeparam>
        /// <param name="propertyName">Имя этого свойства, которую нужно проверить на уникальность</param>
        /// <param name="obj">Содержимое свойства</param>
        /// <param name="selectObj"></param>
        /// <returns></returns>
        public static bool CheckForUniqueness<TClass>(string propertyName, object obj, object selectObj) where TClass : class
        {
            if (selectObj != null)
                if (selectObj.Equals(obj))
                    return false;

            using var db = new InventoryEntities();
            var dbSet = db.Set<TClass>();

            var type = typeof(TClass);

            var parameterExpression = System.Linq.Expressions.Expression.Parameter(type, "tClass");
            var constant = System.Linq.Expressions.Expression.Constant(obj);
            var property = System.Linq.Expressions.Expression.Property(parameterExpression, propertyName);
            var expression = System.Linq.Expressions.Expression.Equal(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<TClass, bool>>(expression, parameterExpression);
            var compiledLambda = lambda.Compile();
            var result = dbSet.FirstOrDefault(compiledLambda);

            return result != null;
        }

        public static void Add<TClass>(TClass value) where TClass : class
        {
            using var db = new InventoryEntities();

            db.Set<TClass>().Add(value);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных. {e.Message}", "Ошибка при добавлении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static void Edit<TClass>(int idObject, TClass updatedObject) where TClass : class
        {
            using var db = new InventoryEntities();

            var existingObject = db.Set<TClass>().Find(idObject);

            if (existingObject == null)
            {
                MessageBox.Show("Ошибка при изменении данных в базе данных. Изменяемый объект не найден в базе данных.", "Ошибка при изменении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            db.Entry(existingObject).CurrentValues.SetValues(updatedObject);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при изменении данных в базе данных. {e.Message}", "Ошибка при изменении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void Delete<TClass>(int idObject) where TClass : class
        {
            using var db = new InventoryEntities();

            var dbSet = db.Set<TClass>();
            var foundObject = dbSet.Find(idObject);

            if (foundObject == null)
            {
                MessageBox.Show("Ошибка при удалении данных в базе данных. Удаляемый объект не найдет в базе данных.", "Ошибка при удаления данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dbSet.Remove(foundObject);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при удалении данных в базе данных. {e.Message}", "Ошибка при удаления данных.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}