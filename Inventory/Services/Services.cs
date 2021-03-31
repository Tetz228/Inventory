namespace Inventory.Services
{
    using Inventory.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;

    using Microsoft.Win32;

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

        public static string ValidInventoryNumber(string inventoryNumberString, int? selectInventoryNumber, dynamic classInventoryNumbers)
        {
            string result;
            classInventoryNumbers.Inventory_number = int.Parse(inventoryNumberString);

            if (selectInventoryNumber == null)
            {
                result = classInventoryNumbers.IsUniqueInventoryNumber(classInventoryNumbers.Inventory_number);
            }
            else
            {
                result = selectInventoryNumber != classInventoryNumbers.Inventory_number ? (string) classInventoryNumbers.IsUniqueInventoryNumber(classInventoryNumbers.Inventory_number) : null;
            }

            return result;
        }

        public static void Add<TClass>(TClass value) where TClass : class
        {
            using var db = new InventoryEntities();

            var dbSet = db.Set<TClass>();
            dbSet.Add(value);

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