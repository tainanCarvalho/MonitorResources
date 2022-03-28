using Interval.Storage.Infrastruct;
using Interval.Storage.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interval.Storage.Factory
{
    public sealed class StorageFactory
    {
        private static Dictionary<string, Type> keyValues = new Dictionary<string, Type>() 
        {
            { "csv", typeof(CsvStorageFile) },
            { "json", typeof(JSONStorageFile) },
            { "yaml", typeof(YamlStorageFile) }
        };

        public StorageFactory()
        {
        }

        public static IStorageData Factory(string extension, string path, string nameFile) 
        {
            keyValues.TryGetValueDefault(extension, typeof(CsvStorageFile), out Type value);
            return (IStorageData)Activator.CreateInstance(value, path, nameFile);
        }
    }

    public static class DictionaryExtention
    {
        public static bool TryGetValueDefault<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> keyValues, Tkey tkey, Tvalue defautValue, out Tvalue value)
        {
            if(!keyValues.TryGetValue(tkey, out Tvalue tvalue))
            {
                value = defautValue;
                return false;
            }

            value = tvalue;
            return true;

        }
    }
}
