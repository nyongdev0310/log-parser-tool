using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LogParser.ConsoleVersion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Log Parser (Console) ===");

            string inputFile = "sample/log.txt";
            string outputFile = "sample/result.csv";

            // Optional: allow args override
            if (args.Length >= 1)
                inputFile = args[0];
            if (args.Length >= 2)
                outputFile = args[1];

            Console.WriteLine($"Input : {inputFile}");
            Console.WriteLine($"Output: {outputFile}");

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Log file not found: {inputFile}");
                return;
            }

            try
            {
                var items = ParseLogFile(inputFile);
                WriteCsv(outputFile, items);

                Console.WriteLine("Parsing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while parsing log file:");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Represents a single parsed log entry.
        /// </summary>
        private class LogItem
        {
            public string Level { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
        }

        /// <summary>
        /// Reads the log file and extracts entries that match:
        /// [LEVEL] message
        /// </summary>
        private static List<LogItem> ParseLogFile(string path)
        {
            var results = new List<LogItem>();
            var pattern = new Regex(@"\[(INFO|WARN|ERROR)\]\s+(.*)", RegexOptions.Compiled);

            foreach (var line in File.ReadLines(path))
            {
                var match = pattern.Match(line);
                if (match.Success)
                {
                    results.Add(new LogItem
                    {
                        Level = match.Groups[1].Value,
                        Message = match.Groups[2].Value
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// Writes parsed log items into a CSV file.
        /// </summary>
        private static void WriteCsv(string path, List<LogItem> items)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Level,Message");

                foreach (var item in items)
                {
                    string safeMessage = item.Message.Replace("\"", "\"\"");
                    writer.WriteLine($"{item.Level},\"{safeMessage}\"");
                }
            }
        }
    }
}
