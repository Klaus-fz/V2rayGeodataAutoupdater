using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Win32;

using V2rayGeodataAutoupdater.Data;

namespace V2rayGeodataAutoupdater.Config.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppConfig savedConfig = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (File.Exists(Constants.CONFIG_FILE))
            {
                try
                {
                    savedConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(Constants.CONFIG_FILE));
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Read config file error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Config file error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            editorConfig = new(savedConfig);
            // Good way is implement INotifyPropertyChanged for AppConfig, but save time here.
            DataPanel.DataContext = editorConfig;
        }

        #region Btn Event

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Constants.CONFIG_FILE))
            {
                try
                {
                    SaveConfigFile($"{AppDomain.CurrentDomain.BaseDirectory}\\{Constants.CONFIG_FILE}");
                    MessageBox.Show("File Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Save config file error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Title = "Save File To",
                    FileName = $"{Constants.CONFIG_FILE}",
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                };

                if (saveFileDialog.ShowDialog().Value)
                {
                    SaveConfigFile(saveFileDialog.FileName);
                    MessageBox.Show("File Saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No file selected", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            editorConfig = new(savedConfig);
            DataPanel.DataContext = editorConfig;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaunchClient(parameter: "-U");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLaunch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaunchClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!{Environment.NewLine}{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Assist Functions

        private void SaveConfigFile(string path = "")
        {
            File.WriteAllText(path, JsonSerializer.Serialize(editorConfig, new JsonSerializerOptions { WriteIndented = true }));
            savedConfig = editorConfig;
        }

        private void LaunchClient(string path = "", string parameter = "")
        {
            if (File.Exists($"{path}{Constants.UPDATER_EXE}"))
            {
                Process.Start($"{path}{Constants.UPDATER_EXE}", parameter);
            }
            else
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "Select Updater Application",
                    FileName = Constants.UPDATER_EXE,
                    Multiselect = false,
                    DefaultExt = ".exe",
                    Filter = "Executable File(.exe)|*.exe",
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    Process.Start(openFileDialog.FileName, parameter);
                }
                else
                {
                    MessageBox.Show("No file selected", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        #endregion

    }
}
