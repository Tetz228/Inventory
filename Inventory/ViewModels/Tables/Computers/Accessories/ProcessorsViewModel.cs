﻿namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.Base;

    public class ProcessorsViewModel : BaseViewModel<Processor>
    {
        public ProcessorsViewModel() : base(Processors, RefreshCollection) => RefreshCollection();
        
        public static ObservableCollection<Processor> Processors { get; set; } = new();
        
        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Processors.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Processors.Sort(processor => processor.Name, SortDirection);
                            break;
                        }
                    case "Сокет":
                        {
                            Processors.Sort(processor => processor.Socket.Name, SortDirection);
                            break;
                        }
                    case "Количество ядер":
                        {
                            Processors.Sort(processor => processor.Amount_cores, SortDirection);
                            break;
                        }
                    case "Базовая частота":
                        {
                            Processors.Sort(processor => processor.Base_frequency + " " + processor.Unit.Short_name, SortDirection);
                            break;
                        }
                }
            }
        }

        public ICommand AddProcessorCommand => new DelegateCommand(() =>
        {
            var addWindow = new ProcessorAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditProcessorCommand => new DelegateCommand<Processor>(processor =>
        {
            var editWindow = new ProcessorEditWindow();
            var editViewModel = new ProcessorEditViewModel(processor);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, processor => processor != null);

        public ICommand DeleteProcessorCommand => new DelegateCommand<Processor>(selectProcessor =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить процессор:\nпроизводитель - {selectProcessor.Manufacturer.Name};\nнаименование - {selectProcessor.Name};\nсокет - {selectProcessor.Socket.Name};\nкол-во ядер - {selectProcessor.Amount_cores};\nбазовая частота - {selectProcessor.Base_frequency} {selectProcessor.Unit.Short_name}?", "Удаление процессора", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Processor>(selectProcessor.Id_processor))
                Processors.Remove(selectProcessor);
        }, selectProcessor => selectProcessor != null);

        public static void RefreshCollection()
        {
            Processors.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Processors.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(socket => socket.Socket))
                Processors.Add(item);

            Processors.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
