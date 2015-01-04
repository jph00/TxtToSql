using System;
using System.IO;
using NUnit.Framework;

namespace TxtToSql {
    [TestFixture]
    public class ReadFiles {
		private const string PATH = @"..\..\..\data\bulldozers1k.csv";

        [Test]
        public void ToSQL() {
            Console.WriteLine(new SqlCreator().CreateSQL(PATH));
        }

        [Test]
        public void Summarise() {
            new SqlCreator().Summarise(PATH);
        }

        [Test]
        public void SummariseAll() {
            foreach (var file in Directory.GetFiles(@"..\..\..\data", "*.csv"))
                new SqlCreator().Summarise(file);
        }
    }
}
