namespace Inventory.Services
{
    using ClosedXML.Report;
    using Inventory.Model;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;
    using System.Windows;
    using LinqExtensions = System.Linq.Expressions;

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

        public static ObservableCollection<TClassSource> Sort<TClassSource, TProperty>(this ObservableCollection<TClassSource> observableCollection, Func<TClassSource, TProperty> property)
        {
            var sortCollection = observableCollection.OrderBy(property).ToList();

            observableCollection.Clear();

            foreach (var item in sortCollection)
                observableCollection.Add(item);

            return observableCollection;
        }

        public static ObservableCollection<TClass> CopyingElements<TClass>(this ObservableCollection<TClass> observableCollection)
        {
            var newCollection = new ObservableCollection<TClass>();

            foreach (var item in observableCollection)
                newCollection.Add(item);

            return newCollection;
        }

        public static bool IsValidationProperties<TClass>(TClass dictionary) where TClass : Dictionary<string, string> => dictionary.Count == 0
                                                                             || dictionary.All(item => item.Value == null);

        public static void ExportExcel(this ICollectionView collectionView, string nameTemplate, string namedAreaName)
        {
            string fileName = IsSavingDocumentExcel();

            if (fileName != null)
            {
                var template = new XLTemplate(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Templates\\" + nameTemplate);
                template.AddVariable(namedAreaName, collectionView.ToDynamicList());
                template.Generate();
                template.SaveAs(fileName);

                Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            }
        }

        private static string IsSavingDocumentExcel()
        {
            var saveExcelDoc = new SaveFileDialog { Filter = "Excel-документ|*.xlsx", Title = "Экспорт данных в Excel-документ" };
            saveExcelDoc.ShowDialog();

            return saveExcelDoc.FileName != "" ? saveExcelDoc.FileName : null;
        }

        /// <summary>
        /// Проверка на уникальность определенного свойства
        /// </summary>
        /// <typeparam name="TClass">Класс, который содержит свойство</typeparam>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="obj">Cвойство</param>
        /// <param name="selectObj">Необязательный аргумент, которое нужно, если открыты окна с изменением информации</param>
        /// <returns>Возвращает true, если свойство не уникально, иначе false</returns>
        public static bool CheckForUniqueness<TClass>(string propertyName, object obj, object selectObj) where TClass : class
        {
            if (selectObj != null)
                if (selectObj.Equals(obj))
                    return false;

            using var db = new InventoryEntities();
            var dbSet = db.Set<TClass>();

            var type = typeof(TClass);

            var parameterExpression = LinqExtensions.Expression.Parameter(type, "tClass");
            var constant = LinqExtensions.Expression.Constant(obj);
            var property = LinqExtensions.Expression.Property(parameterExpression, propertyName);
            var expression = LinqExtensions.Expression.Equal(property, constant);
            var lambda = LinqExtensions.Expression.Lambda<Func<TClass, bool>>(expression, parameterExpression);
            var compiledLambda = lambda.Compile();
            var result = dbSet.FirstOrDefault(compiledLambda);

            return result != null;
        }

        public static bool IsSaveDatabase { get; set; }

        public static bool Add<TClass>(TClass value) where TClass : class
        {
            using var db = new InventoryEntities();

            db.Set<TClass>().Add(value);

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при добавлении данных в базу данных.\n{e.Message}", "Ошибка при добавлении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool Edit<TClass>(int idObject, TClass updatedObject) where TClass : class
        {
            using var db = new InventoryEntities();

            var existingObject = db.Set<TClass>().Find(idObject);

            if (existingObject == null)
            {
                MessageBox.Show("Ошибка при изменении данных в базе данных. Изменяемый объект не найден в базе данных.", "Ошибка при изменении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            db.Entry(existingObject).CurrentValues.SetValues(updatedObject);

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при изменении данных в базе данных.\n{e.Message}", "Ошибка при изменении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool Delete<TClass>(int idObject) where TClass : class
        {
            using var db = new InventoryEntities();

            var dbSet = db.Set<TClass>();
            var foundObject = dbSet.Find(idObject);

            if (foundObject == null)
            {
                MessageBox.Show("Ошибка при удалении данных в базе данных. Удаляемый объект не найден в базе данных.", "Ошибка при удалении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            dbSet.Remove(foundObject);

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при удалении данных в базе данных.\n{e.Message}", "Ошибка при удалении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}