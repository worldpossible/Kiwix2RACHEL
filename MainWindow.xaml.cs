using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace Kiwix2RACHEL
{
    public partial class MainWindow : Window
    {
        // Instance of our AppData class for storing our data
        private AppData m_appData;

        // Our preview window
        private Preview m_previewWindow;

        public MainWindow()
        {
            InitializeComponent();

            // Create our AppData instance
            m_appData = new AppData();

            // Set the datacontext our appData instance. This will let our UI update via the bindings 
            // and the INotifyPropertyChanged implementation 
            DataContext = m_appData;
        }

        // Click handler for the logo selection browse button
        private void LogoBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open a file browser for the user to select a png file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "Image Files(*.png;)|*.png;";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Set the logo path on our AppData instance to the selected file
                    m_appData.LogoPath = openFileDialog.FileName;
                }
            }
        }

        // Click handler for the ZIM browse button
        private void ZimBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Open a file browser for the user to select a ZIM file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Filter = "zim files (*.zim)|*.zim";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Set the ZIM path on our AppData instance
                    m_appData.ZimPath = openFileDialog.FileName;
                }
            }
        }

        // Click handler for the clear button
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("This will reset all values and clear your project. Are you sure you want to proceed?", "Clear Project", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Reset the values on our AppData instance
                // Could also create a new instance here
                m_appData.Reset();
            }
        }

        // Click handler for the preview button
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!m_appData.CheckArgs()){
                return;
            }

            // Check if we already have a preview window instance
            if (m_previewWindow != null)
            {
                // Close it if we do
                m_previewWindow.Close();
            }

            // Get a template HTML string from our AppData instance to show in the preview window
            string template = m_appData.BuildPreview();

            // Create the new preview window. We pass it our string template
            m_previewWindow = new Preview(template);

            // Show the new window 
            m_previewWindow.Show();
        }

        // Click handler for the save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check that all the necessary values are set
            if (!m_appData.CheckArgs())
            {
                return;
            }

            // The folder path we will save to
            var folderPath = string.Empty;

            // Open a folder browser for the location to save at
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.RootFolder  = Environment.SpecialFolder.Desktop;
                folderDialog.Description = "Select the directory to save your module to";
                var result               = folderDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    folderPath = folderDialog.SelectedPath;
                }
            }

            // If a folder wasn't selected we return
            if (folderPath == string.Empty)
            {
                return;
            }

            // Get the selected language code
            Tuple<string, string> folderLanguage = m_appData.Languages[m_appData.LanguageIndex];

            // Build the module folder name
            string folderName = folderLanguage.Item2 + "-" + m_appData.Name;

            // Path to the new module dir 
            string moduleDir = Path.Combine(folderPath, folderName);

            // If a folder wasn't selected we return
            if (Directory.Exists(moduleDir))
            {
                // Ask if the user wants to delete the existing module
                if (System.Windows.MessageBox.Show("A module with this name already exists in this directory. Do you want to overwrite it?", "Module Exists Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Directory.Delete(moduleDir);
                }
                else
                {
                    return;
                }
            }


            // Get our index file as a list of lines from our AppData instance
            List<string> indexLines = m_appData.BuildIndex();

            // If we didn't get a result, error message and exit
            if (indexLines == null) {
                System.Windows.MessageBox.Show("Failed to generate the index lines");
                return;
            }





            // Create the new module directory
            Directory.CreateDirectory(moduleDir);

            // Create the data path in the new module directory
            string dataDir = Path.Combine(moduleDir, "data");

            // Create the data directory
            Directory.CreateDirectory(dataDir);

            // Create the content path in data for the ZIM
            string contentDir = Path.Combine(dataDir, "content");

            // Create the content directory
            Directory.CreateDirectory(contentDir);

            // Create the output path for the rachel-index.php
            string indexPath = Path.Combine(moduleDir, "rachel-index.php");

            // Write all of the lines to a file in the module directory
            File.WriteAllLines(indexPath, indexLines, Encoding.UTF8);

            // Create our new logo path with the name logo.png
            string moduleLogoPath = Path.Combine(moduleDir, "logo.png");

            // Copy the logo to the output folder
            File.Copy(m_appData.m_logoPath, moduleLogoPath);

            string moduleZimPath = Path.Combine(contentDir, Path.GetFileName(m_appData.ZimPath));

            // Ask if the user wants to move the zim file. 
            if (System.Windows.MessageBox.Show("Do you want to move the ZIM file to the new directory?", "Move ZIM Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Move the zim file to the output folder 
                File.Move(m_appData.m_zimPath, moduleZimPath);
            }

            // We're done
            System.Windows.MessageBox.Show("Module successfully created at " + folderPath, "Module Created");
        }

        private void LanguageCode_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
