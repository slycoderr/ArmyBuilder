using System;
using Slycoder.MVVM;
using SQLite;

namespace Slycoder.DataAccessLayer
{
    public enum DatabaseOperation
    {
        Add,
        Remove,
        Modify
    }

    public abstract class Database : BindableBase
    {
        protected bool Loaded { get; set; }
        protected SQLiteConnection StaticDatabase;
        protected SQLiteConnection UserDatabase;

        public void PerformOperation(object obj, DatabaseOperation operation, bool onUserDatabase = true)
        {
            if (UserDatabase != null && onUserDatabase)
            {
                switch (operation)
                {
                    case DatabaseOperation.Add:
                        UserDatabase.Insert(obj);
                        break;
                    case DatabaseOperation.Remove:
                        UserDatabase.Delete(obj);
                        break;
                    case DatabaseOperation.Modify:
                        UserDatabase.Update(obj);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
                }
            }

            else if (StaticDatabase != null && !onUserDatabase)
            {
                switch (operation)
                {
                    case DatabaseOperation.Add:
                        StaticDatabase.Insert(obj);
                        break;
                    case DatabaseOperation.Remove:
                        StaticDatabase.Delete(obj);
                        break;
                    case DatabaseOperation.Modify:
                        StaticDatabase.Update(obj);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
                }
            }
        }
    }
}