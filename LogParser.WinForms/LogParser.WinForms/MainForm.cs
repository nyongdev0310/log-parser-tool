using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogParser.WinForms
{
    public partial class MainForm : Form
    {
        private TextBox txtLogPath;
        private TextBox txtOutputPath;
        private Button btnBrowseLog;
        private Button btnBrowseOutput;
        private Button btnParse;
        private TextBox txtLog;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Log Parser & Analyzer";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblInput = new Label
            {
                Text = "Log File:",
                Left = 20,
                Top = 20,
                Width = 80
            };

            txtLogPath = new TextBox
            {
                Left = 100,
                Top = 18,
                Width = 360
            };

            btnBrowseLog = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 16,
                Width = 90
            };
            btnBrowseLog.Click += BtnBrowseLog_Click;

            var lblOutput = new Label
            {
                Text = "CSV Output:",
                Left = 20,
                Top = 60,
                Width = 80
            };

            txtOutputPath = new TextBox
            {
                Left = 100,
                Top = 58,
                Width = 360
            };

            btnBrowseOutput = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 56,
                Width = 90
            };
            btnBrowseOutput.Click += BtnBrowseOutput_Click;

            btnParse = new Button
            {
                Text = "Parse",
                Left = 20,
                Top = 100,
                Width = 540,
                Height = 30
            };
            btnParse.Click += BtnParse_Click;

            txtLog = new TextBox
            {
                Left = 20,
                Top = 150,
                Width = 540,
                Height = 180,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            this.Controls.Add(lblInput);
            this.Controls.Add(txtLogPath);
            this.Controls.Add(btnBrowseLog);
            this.Controls.Add(lblOutput);
            this.Controls.Add(txtOutputPath);
            this.Controls.Add(btnBrowseOutput);
            this.Controls.Add(btnParse);
            this.Controls.Add(txtLog);
        }

        private void BtnBrowseLog_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtLogPath.Text = ofd.FileName;

                    string csvPath = Path.ChangeExtension(ofd.FileName, ".csv");
                    txtOutputPath.Text = csvPath;
                }
            }
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                if (!string.IsNullOrWhiteSpace(txtOutputPath.Text))
                {
                    sfd.FileName = Path.GetFileName(txtOutputPath.Text);
                    sfd.InitialDirectory = Path.GetDirectoryName(txtOutputPath.Text);
                }

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = sfd.FileName;
                }
            }
        }

        private void BtnParse_Click(object sender, EventArgs e)
        {
            txtLog.Clear();

            string inputFile = txtLogPath.Text;
            string outputFile = txtOutputPath.Text;

            if (!File.Exists(inputFile))
            {
                Log("Log file not found.");
                MessageBox.Show("Log file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(outputFile))
            {
                Log("CSV output path is empty.");
                MessageBox.Show("Please select a CSV output file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Log("Parsing log file...");
                var items = ParseLogFile(inputFile);

                Log("Writing CSV...");
                WriteCsv(outputFile, items);

                Log("Done: " + outputFile);
                MessageBox.Show("Parsing completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private List<LogItem> ParseLogFile(string path)
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
        private void WriteCsv(string path, List<LogItem> items)
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

        private void Log(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }
}
