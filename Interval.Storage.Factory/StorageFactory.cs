﻿using Interval.Storage.Interface;
using Interval.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Interval.Storage.Factory
{
    public sealed class StorageFactory
    {
        private static Dictionary<string, Type> keyValues = new Dictionary<string, Type>() 
        {
            { "csv", typeof(CsvStorageFile) },
            { "json", typeof(JsonStorageFile) },
            { "yaml", typeof(YamlStorageFile) }
        };

        [ExcludeFromCodeCoverage]
        private StorageFactory()
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
