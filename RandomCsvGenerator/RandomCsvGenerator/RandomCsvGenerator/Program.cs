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
                Console.WriteLine("I Except to get: <path to folder> <number of lines> <output_folder>");
                return;
            }
            string path = args[0];
            if (!int.TryParse(args[1], out var lines))
            {
                Console.WriteLine("Second argument should be a number");
                return;
            }
            string outputFolder = args[2];
            if (!Directory.Exists(outputFolder))
            {
                Console.WriteLine("Directory doesn't exist, create it first and then re run");
                return;
            }

            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).ToList();
            var linesToTakeFromEachFile = lines / files.Count;
            Console.WriteLine(linesToTakeFromEachFile);
            StringBuilder stringBuilder = new StringBuilder();
            var index = 0;
            foreach (var file in files)
            {
                var fileLines = File.ReadAllLines(file);
                for (int i = 0; i < linesToTakeFromEachFile; i++)
                {
                    var lineIndex = new Random().Next(0, fileLines.Length);
                    stringBuilder.AppendLine(fileLines[lineIndex]);
                }
                index++;
                Console.WriteLine($"Finished {index}/{files.Count}");
            }

            var outputPath = Path.Combine(outputFolder, "radnom-csv.csv");
            File.WriteAllText(outputPath, stringBuilder.ToString());
        }
    }
}
