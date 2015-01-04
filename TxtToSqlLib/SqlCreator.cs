using System;
using System.IO;
using System.Linq;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace TxtToSql {
    public class SqlCreator {
        public string CreateSQL(string fName) {
            var res = new StringBuilder();
            if (!File.Exists(fName)) return "";
            var data = GetDataFile(fName);
            res.AppendFormat("CREATE TABLE {0} (\n", Path.GetFileNameWithoutExtension(fName));

            var cols = string.Join(",\n", data.Cols.Select(o => o.CreateSQL).ToArray());

            res.AppendFormat("{0}\n);\n\n", cols);
            return res.ToString();
        }

        public void CreateAllSQL(string path) {
            foreach (var file in Directory.GetFiles(path, "*.csv"))
                Console.WriteLine(CreateSQL(file));
        }

        public void SummariseAll(string path) {
            foreach (var file in Directory.GetFiles(path, "*.csv"))
                Summarise(file);
        }

        public void Summarise(string fName) {
            if (!File.Exists(fName)) return;
            var data = GetDataFile(fName);

            Console.WriteLine("File name: " + Path.GetFileName(fName));
            Console.WriteLine(data.Count + " recs\n----\n");
            Console.WriteLine("Columns: \n----\n");
            foreach (var col in data.Cols) Console.WriteLine(col);

            Console.WriteLine("\nUnique values per column: \n----\n");
	        foreach (var col in data.Cols)
				Console.WriteLine(col.Name + ": " + col.MostCommonSumm(20));
//                Console.WriteLine(col.Name + ": " + string.Join(";", col.ValCount.Select(o => o.Key).ToArray()));
            Console.WriteLine("--------\n\n");
        }

        private static DataFile GetDataFile(string path) {
            using (var csv = new CsvReader(new StreamReader(path), false)) {
                var lines = csv.GetEnumerator();

                lines.MoveNext();
                var fields = lines.Current;
                var data = new DataFile(fields);
                while (lines.MoveNext()) data.Add(lines.Current);
                return data;
            }
        }
    }
}
