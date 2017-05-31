using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ArmyBuilder.Core.Database
{
    public class Utility
    {
        public static bool IsIgnoredProperty(Type t, string propertyName)
        {
            return t?.GetRuntimeProperty(propertyName)?.GetCustomAttributes(typeof(IgnoreAttribute))?.FirstOrDefault() != null;
            //return (t.GetProperties().ToList().FirstOrDefault(m => m.Name == propertyName)?.GetCustomAttributes(typeof (IgnoreAttribute), true)).Any();
        }

        public static bool IsUserData(Type t)
        {
            return t.GetTypeInfo().GetCustomAttributes().ToList().Contains(new UserDataAttribute());
        }
    }
    //Attribute
    public class UserDataAttribute : Attribute
    {

    }
}
