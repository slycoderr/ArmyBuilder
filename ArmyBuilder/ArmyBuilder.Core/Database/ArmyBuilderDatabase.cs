using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ArmyBuilder.Core.Models;
using ArmyBuilder.Core.ViewModels;
using MoreLinq;
using Slycoder.DataAccessLayer;
using Slycoder.MVVM;
using SQLite;

namespace ArmyBuilder.Core.Database
{
    public class ArmyBuilderDatabase : Slycoder.DataAccessLayer.Database
    {
        public ObservableCollection<ArmyList> ArmyLists { get; } = new ObservableCollection<ArmyList>();
        public ObservableCollection<ArmyListData> ArmyListData { get; } = new ObservableCollection<ArmyListData>();
        public ObservableCollection<DetachmentData> ArmyListDetachments { get; } = new ObservableCollection<DetachmentData>();

        public void Load(string path)
        {
            if (!Loaded)
            {
                UserDatabase = new SQLiteConnection(Path.Combine(path, "UserData.db"));
                //open and/or create the database

                //UserDatabase.DropTable<ArmyList>();
                //UserDatabase.DropTable<ArmyListData>();

                //UserDatabase.CreateTable<ArmyList>(); // make sure tables exists
                //UserDatabase.CreateTable<ArmyListData>();
                //UserDatabase.CreateTable<DetachmentData>();

                //var armylists = UserDatabase.Table<ArmyList>();
                //var armyData = UserDatabase.Table<ArmyListData>();
                //var detachments = UserDatabase.Table<DetachmentData>();
                ////bool doneAdding = false;


                ////MainViewModel.UiContext.Post(a =>
                ////{
                //    armylists.ForEach(l => ArmyLists.Add(l));
                //    armyData.ForEach(l => ArmyListData.Add(l));
                //detachments.ForEach(l => ArmyListDetachments.Add(l));
                ////    doneAdding = true;
                ////}, null);

                ////while (!doneAdding) { }

                //ArmyLists.CollectionChanged += OnCollectionChanged;
                //ArmyListData.CollectionChanged += OnCollectionChanged;
                //ArmyListDetachments.CollectionChanged += OnCollectionChanged;

                //ArmyLists.ForEach(a => a.PropertyChanged += OnPropertyChanged);
                //ArmyListData.ForEach(a => a.PropertyChanged += OnPropertyChanged);
                //ArmyListDetachments.ForEach(a => a.PropertyChanged += OnPropertyChanged);

                Loaded = true;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Loaded)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.Cast<BindableBase>().ForEach(l => PerformOperation(l, DatabaseOperation.Add, Utility.IsUserData(l.GetType())));
                        e.NewItems.Cast<BindableBase>().ForEach(l => l.PropertyChanged += OnPropertyChanged);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems.Cast<BindableBase>().ForEach(l => PerformOperation(l, DatabaseOperation.Remove, Utility.IsUserData(l.GetType())));
                        e.OldItems.Cast<BindableBase>().ForEach(l => l.PropertyChanged -= OnPropertyChanged);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Utility.IsIgnoredProperty(sender.GetType(), e.PropertyName) && Loaded)
            {
                PerformOperation(sender, DatabaseOperation.Modify, Utility.IsUserData(sender.GetType()));
            }
        }
    }
}