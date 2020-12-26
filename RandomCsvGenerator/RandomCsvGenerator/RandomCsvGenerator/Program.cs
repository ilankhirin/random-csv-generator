using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RandomCsvGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("I Except to get: <input_file> <ids_file> <output_folder>");
                return;
            }
            string inputFile = args[0];
            string idsFile = args[1];
            if (!File.Exists(idsFile))
            {
                Console.WriteLine("Ids file path is invalid " + idsFile);
                return;
            }
            string outputFolder = args[2];
            if (!Directory.Exists(outputFolder))
            {
                Console.WriteLine("Directory doesn't exist, create it first and then re run");
                return;
            }

            var outputPath = Path.Combine(outputFolder, "ids-csv.csv");
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            var idsToNumberOfOccurrences = ConstructIdsDictionary(idsFile);

            List<string> lines = new List<string>();
            var index = 0;
            foreach (var line in File.ReadLines(inputFile))
            {
                var id = line.Split(';')[1];
                if (idsToNumberOfOccurrences.ContainsKey(id))
                {
                    index++;
                    idsToNumberOfOccurrences[id]++;
                    lines.Add(line);
                    if (lines.Count >= 1000)
                    {
                        File.AppendAllLines(outputPath, lines);
                        lines.Clear();
                        Console.WriteLine($"{index} lines written");
                    }
                }
            }

            File.AppendAllLines(outputPath, lines);

            var occurrncesContent = idsToNumberOfOccurrences.Select(x => $"{x.Key},{x.Value}");
            var occurrncesFilePath = Path.Combine(outputFolder, "occurrnces.csv");
            if (File.Exists(occurrncesFilePath))
            {
                File.Delete(occurrncesFilePath);
            }
            File.WriteAllText(occurrncesFilePath, string.Join("\n", occurrncesContent));
        }

        static Dictionary<string, int> ConstructIdsDictionary(string idsFile)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (var line in File.ReadAllLines(idsFile))
            {
                dict.Add(line, 0);
            }

            return dict;
        }
    }
}
