using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        private const string RecentFilesPath = "recent_files.txt";
        private List<string> recentFiles;

        public MainWindow()
        {
            InitializeComponent();
            recentFiles = new List<string>(); 
            LoadRecentFiles();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                EditorTextBox.Text = File.ReadAllText(filePath);
                AddToRecentFiles(filePath);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, EditorTextBox.Text);
                AddToRecentFiles(saveFileDialog.FileName);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WordCount_Click(object sender, RoutedEventArgs e)
        {
            string text = EditorTextBox.Text;
            int wordCount = text.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            WordCountTextBlock.Text = $"Words: {wordCount}";
        }

        private void ReplaceText_Click(object sender, RoutedEventArgs e)
        {
            string findText = FindTextBox.Text;
            string replaceText = ReplaceTextBox.Text;

            if (!string.IsNullOrWhiteSpace(findText))
            {
                EditorTextBox.Text = EditorTextBox.Text.Replace(findText, replaceText);
            }
        }

        private void LoadRecentFiles()
        {
            if (File.Exists(RecentFilesPath))
            {
                recentFiles = File.ReadAllLines(RecentFilesPath).ToList();
                UpdateRecentFilesMenu();
            }
        }

        private void SaveRecentFiles()
        {
            File.WriteAllLines(RecentFilesPath, recentFiles);
        }

        private void AddToRecentFiles(string filePath)
        {
            if (!recentFiles.Contains(filePath))
            {
                recentFiles.Insert(0, filePath);
                if (recentFiles.Count > 5) 
                {
                    recentFiles.RemoveAt(recentFiles.Count - 1);
                }
                SaveRecentFiles();
                UpdateRecentFilesMenu();
            }
        }

        private void UpdateRecentFilesMenu()
        {
            RecentFilesMenuItem.Items.Clear();  
            foreach (var file in recentFiles)
            {
                var menuItem = new MenuItem
                {
                    Header = file
                };
                menuItem.Click += (sender, e) => OpenRecentFile(file);
                RecentFilesMenuItem.Items.Add(menuItem);
            }
        }

        private void OpenRecentFile(string filePath)
        {
            EditorTextBox.Text = File.ReadAllText(filePath);
        }

        private void ClearRecentFiles_Click(object sender, RoutedEventArgs e)
        {
            recentFiles.Clear();
            SaveRecentFiles();
            UpdateRecentFilesMenu();
        }
    }
}
