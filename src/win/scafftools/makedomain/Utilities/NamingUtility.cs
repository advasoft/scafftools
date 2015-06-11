using System;

namespace makedomain.Utilities
{
    public static class NamingUtility
    {
        public static string AddSToName(string name)
        {
            var newName = name;

            if (newName.ToLower().EndsWith("s"))
            {
                newName += "es";
            }
            else if (newName.ToLower().EndsWith("y"))
            {
                newName = newName.Remove(newName.Length - 1);
                newName += "ies";
            }
            else
            {
                newName += "s";
            }

            return newName;
        }

        public static string RemoveSFromName(string name)
        {
            var newName = name;
            if (newName.ToLower().EndsWith("ses"))
                newName = newName.Remove(newName.Length - 1);
            else if (newName.ToLower().EndsWith("ies"))
            {
                newName = newName.Remove(newName.Length - 3);
                newName = newName + "y";
            }
            else if (newName.ToLower().EndsWith("s"))
                newName = newName.Remove(newName.Length - 1);
            return newName;
        }

        public static string RemoveIdFromName(string name)
        {
            var propertyName = name;
            if (propertyName.ToLower().EndsWith("id"))
            {
                propertyName = propertyName.Remove(propertyName.Length - 2);
            }
            else if (propertyName.ToLower().EndsWith("_id"))
            {
                propertyName = propertyName.Remove(propertyName.Length - 3);
            }
            return propertyName;
        }

        public static string GetCleanTableName(string name)
        {
            var nameArray = name.Split('.');
            if (nameArray.Length > 1)
                return nameArray[nameArray.Length - 1];

            return name;
        }
        public static string GetFullNamespace(string rootNamespace, string tableName)
        {
            string @namespace = rootNamespace;
            var nameArray = tableName.Split('.');
            if (nameArray.Length > 1)
            { 
                for (int i = 0; i < nameArray.Length - 1; i++)
                {
                    @namespace = string.Concat(@namespace, ".", nameArray[i]);
                }
            }

            return @namespace;
        }

    }
}
