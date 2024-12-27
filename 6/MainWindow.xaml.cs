using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;

namespace SystemInfoApp
{
    public partial class MainWindow : Window
    {
        private Timer _updateTimer;

        public MainWindow()
        {
            InitializeComponent();

            _updateTimer = new Timer(5000);
            _updateTimer.Elapsed += UpdateData;
            _updateTimer.Start();

            UpdateData(null, null);
        }

        private void UpdateData(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateMemoryInfo();
                UpdateProcessesInfo();
            });
        }

        private void UpdateMemoryInfo()
        {
            try
            {
                var memoryInfo = new List<object>();
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c systeminfo",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8
                    }
                };

                process.Start();
                using (var reader = process.StandardOutput)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            memoryInfo.Add(new { Parameter = parts[0].Trim(), Value = parts[1].Trim() });
                        }
                    }
                }

                MemoryDataGrid.ItemsSource = memoryInfo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки информации о памяти: {ex.Message}");
            }
        }

        private void UpdateProcessesInfo()
        {
            try
            {
                var processes = Process.GetProcesses()
                    .Select(p => new
                    {
                        PID = p.Id,
                        Name = p.ProcessName,
                        CPU = GetCpuUsage(p),
                        Memory = p.WorkingSet64 / (1024 * 1024)
                    })
                    .OrderByDescending(p => p.CPU)
                    .ToList();

                ProcessDataGrid.ItemsSource = processes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки процессов: {ex.Message}");
            }
        }

        private double GetCpuUsage(Process process)
        {
            return 0.0;
        }
    }
}
