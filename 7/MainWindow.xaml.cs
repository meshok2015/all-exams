using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace ImageFileViewer
{
    public partial class MainWindow : Window
    {
        private const string FilePath = "filelist.txt";
        private Dictionary<string, string> filePaths = new Dictionary<string, string>(); 
        private List<string> fileNames = new List<string>(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    string fileName = Path.GetFileName(filePath);
                    if (!filePaths.ContainsKey(fileName)) 
                    {
                        filePaths[fileName] = filePath;
                        fileNames.Add(fileName); 
                    }
                }
                RefreshFileList();
            }
        }

        private void SaveFiles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllLines(FilePath, fileNames);
                MessageBox.Show("File names saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var loadedNames = File.ReadAllLines(FilePath);
                    foreach (var name in loadedNames)
                    {
                        if (!fileNames.Contains(name))
                        {
                            fileNames.Add(name);
                        }
                    }
                    RefreshFileList();
                }
                else
                {
                    MessageBox.Show("File list not found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (FileListBox.SelectedItem != null)
            {
                string selectedFile = FileListBox.SelectedItem.ToString();
                fileNames.Remove(selectedFile);
                filePaths.Remove(selectedFile); 
                RefreshFileList();
            }
            else
            {
                MessageBox.Show("Please select a file to delete.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FileListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FileListBox.SelectedItem != null)
            {
                string selectedFile = FileListBox.SelectedItem.ToString();
                if (filePaths.ContainsKey(selectedFile) && File.Exists(filePaths[selectedFile]))
                {
                    try
                    {
                        ImageViewer.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(filePaths[selectedFile]));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("File does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void RefreshFileList()
        {
            FileListBox.ItemsSource = null;
            FileListBox.ItemsSource = fileNames;
        }
    }
}
