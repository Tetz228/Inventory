namespace Inventory
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

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
    }
}
