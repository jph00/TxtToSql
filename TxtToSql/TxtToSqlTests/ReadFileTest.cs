using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO.Csv;
using NUnit.Framework;

namespace TxtToSql {
    [TestFixture]
    public class ReadFiles {
        private const string PATH = @"C:\Docs\Dropbox\Grameen Foundation\data_clean\";

        [Test]
        public void ToSQL() {
            Console.WriteLine(new SqlCreator().CreateSQL(PATH + "CKW_Report.csv"));
        }

        [Test]
        public void Summarise() {
            new SqlCreator().Summarise(PATH + "Search_Logs.csv");
        }

        [Test]
        public void SummariseAll() {
            foreach (var file in Directory.GetFiles(@"C:\Docs\Dropbox\Grameen Foundation\data", "*.csv"))
                new SqlCreator().Summarise(file);
        }
    }
}
