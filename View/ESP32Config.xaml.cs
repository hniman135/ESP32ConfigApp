using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Page_Navigation_App.View
{
    /// <summary>
    /// Interaction logic for ESP32Config.xaml
    /// </summary>
    public partial class ESP32Config : UserControl
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isOperationInProgress = false;

        public ESP32Config()
        {
            InitializeComponent();
            LoadComPorts();

            // Wire up events
            RefreshPortsButton.Click += (s, e) => LoadComPorts();
            ConnectButton.Click += ConnectButton_Click;
            SaveConfigButton.Click += SaveConfigButton_Click;
            UploadFirmwareButton.Click += UploadFirmwareButton_Click;
        }

        private void LoadComPorts()
        {
            ComPortComboBox.Items.Clear();
            foreach (string port in SerialPort.GetPortNames())
            {
                ComPortComboBox.Items.Add(port);
            }

            if (ComPortComboBox.Items.Count > 0)
                ComPortComboBox.SelectedIndex = 0;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isOperationInProgress)
            {
                CancelOperation();
                return;
            }

            try
            {
                _isOperationInProgress = true;
                _cancellationTokenSource = new CancellationTokenSource();
                UpdateUIForOperation("connect");

                LogToConsole("Connecting to ESP32...");

                // Simulate connection process
                await Task.Delay(5000, _cancellationTokenSource.Token);

                LogToConsole("Connected to ESP32 successfully.");
            }
            catch (OperationCanceledException)
            {
                LogToConsole("Connection operation canceled.");
            }
            catch (Exception ex)
            {
                LogToConsole($"Connection error: {ex.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
                UpdateUIForOperation(null);
            }
        }

        private async void SaveConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isOperationInProgress)
            {
                CancelOperation();
                return;
            }

            try
            {
                _isOperationInProgress = true;
                _cancellationTokenSource = new CancellationTokenSource();
                UpdateUIForOperation("save");

                LogToConsole("Saving configuration to ESP32...");

                // Simulate saving configuration
                await Task.Delay(3000, _cancellationTokenSource.Token);

                LogToConsole("Configuration saved successfully.");
            }
            catch (OperationCanceledException)
            {
                LogToConsole("Save configuration operation canceled.");
            }
            catch (Exception ex)
            {
                LogToConsole($"Save configuration error: {ex.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
                UpdateUIForOperation(null);
            }
        }

        private async void UploadFirmwareButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isOperationInProgress)
            {
                CancelOperation();
                return;
            }

            try
            {
                _isOperationInProgress = true;
                _cancellationTokenSource = new CancellationTokenSource();
                UpdateUIForOperation("upload");

                LogToConsole("Uploading firmware to ESP32...");

                // Simulate firmware upload with progress
                for (int i = 0; i <= 100; i += 10)
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    LogToConsole($"Upload progress: {i}%");
                    await Task.Delay(500, _cancellationTokenSource.Token);
                }

                LogToConsole("Firmware uploaded successfully.");
            }
            catch (OperationCanceledException)
            {
                LogToConsole("Firmware upload canceled.");
            }
            catch (Exception ex)
            {
                LogToConsole($"Firmware upload error: {ex.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
                UpdateUIForOperation(null);
            }
        }

        private void CancelOperation()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                LogToConsole("Canceling operation...");
            }
        }

        private void UpdateUIForOperation(string operation)
        {
            if (operation == null)
            {
                // Reset to normal state
                ConnectButton.Content = "Connect";
                SaveConfigButton.Content = "Save Configuration";
                UploadFirmwareButton.Content = "Upload Firmware";

                // Enable all buttons
                ConnectButton.IsEnabled = true;
                SaveConfigButton.IsEnabled = true;
                UploadFirmwareButton.IsEnabled = true;
            }
            else
            {
                // Set the active button to "Cancel"
                switch (operation)
                {
                    case "connect":
                        ConnectButton.Content = "Cancel";
                        SaveConfigButton.IsEnabled = false;
                        UploadFirmwareButton.IsEnabled = false;
                        break;
                    case "save":
                        SaveConfigButton.Content = "Cancel";
                        ConnectButton.IsEnabled = false;
                        UploadFirmwareButton.IsEnabled = false;
                        break;
                    case "upload":
                        UploadFirmwareButton.Content = "Cancel";
                        ConnectButton.IsEnabled = false;
                        SaveConfigButton.IsEnabled = false;
                        break;
                }
            }
        }

        private void LogToConsole(string message)
        {
            ConsoleTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            ConsoleTextBox.ScrollToEnd();
        }
    }
}
