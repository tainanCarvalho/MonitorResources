using Interval.Storage.Factory;
using Interval.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Interval.Test
{
    [TestClass]
    public sealed class FactoryTest
    {
        [TestMethod]
        [Description("Se não encontra no dicionario, retorna o valor default.")]
        public void TestDefaultDictionary()
        {
            var obj = typeof(StorageFactory).GetField("keyValues", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

           var keyValue = (Dictionary<string, Type>)obj;

            keyValue.TryGetValueDefault("abc", typeof(CsvStorageFile), out Type value);

            Assert.AreEqual(value, typeof(CsvStorageFile));
        }

        [TestMethod]
        [Description("Retorna o tipo a partir de uma extension.")]
        public void TestGetDictionary()
        {
            var obj = typeof(StorageFactory).GetField("keyValues", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            var keyValue = (Dictionary<string, Type>)obj;

            keyValue.TryGetValueDefault("json", typeof(CsvStorageFile), out Type value);

            Assert.AreEqual(value, typeof(JSONStorageFile));
        }


        [TestMethod]
        [Description("Cria uma instância a paritr de uma extension.")]
        public void TestFactory()
        {
            var result = StorageFactory.Factory("csv", "test", "nome_do_arquivo");

            Assert.IsInstanceOfType(result, typeof(CsvStorageFile));
        }

    }
}
