using System;

namespace TxtToSql {
    internal class Program {
        private static void Main(string[] args) {
            bool show_help = false, do_summarize = false;

            var p = new OptionSet {
                                      { "h|help", "show this message and exit",
                                          v => show_help = v != null },
                                      { "s|summarize", "create metadata summary instead of CREATE SQL",
                                          v => do_summarize = v != null },
                                  };

            try {
                p.Parse(args);
            }
            catch (OptionException e) {
                Console.Write("TxtToSql: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `TxtToSql --help' for more information.");
                return;
            }

            if (show_help) {
                ShowHelp(p);
            } else {
                var t = new SqlCreator();
                string dir = Environment.CurrentDirectory;
                if (do_summarize) t.SummariseAll(dir);
                else t.CreateAllSQL(dir);
            }
        }

        private static void ShowHelp(OptionSet p) {
            Console.WriteLine("Usage: TxtToSql [OPTIONS]+ ");
            Console.WriteLine("Create SQL CREATE scripts based on text files.");
            Console.WriteLine("Uses all sql files in current directory by default.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}