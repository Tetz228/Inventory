﻿namespace Inventory.Services
{
    using ClosedXML.Report;
    using Inventory.Model;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
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

        public static bool Add<TClass>(TClass value) where TClass : class
        {
            var db = new InventoryEntities();

            db.Set<TClass>().Add(value);

            return DbSaveChanges(ref db);
        }

        public static void Edit<TClass>(int idObject, TClass updatedObject) where TClass : class
        {
            var db = new InventoryEntities();

            var existingObject = db.Set<TClass>().Find(idObject);

            if (existingObject == null)
            {
                MessageBox.Show("Ошибка при изменении данных в базе данных. Изменяемый объект не найден в базе данных.", "Ошибка при изменении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            db.Entry(existingObject).CurrentValues.SetValues(updatedObject);

            DbSaveChanges(ref db);
        }

        public static bool Delete<TClass>(int idObject) where TClass : class
        {
            var db = new InventoryEntities();

            var dbSet = db.Set<TClass>();
            var foundObject = dbSet.Find(idObject);

            if (foundObject == null)
            {
                MessageBox.Show("Ошибка при удалении данных в базе данных. Удаляемый объект не найден в базе данных.", "Ошибка при удалении данных.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            dbSet.Remove(foundObject);

            return DbSaveChanges(ref db);
        }

        private static bool DbSaveChanges(ref InventoryEntities db)
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Сбой при сохранении изменений в базе данных.", "Сбой при сохранении в базу данных.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Сбой при проверке сущностей.", "Сбой при сохранении данных в базу данных.",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}