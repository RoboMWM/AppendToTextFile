using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppendToTextFile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AppendButton_Click(object sender, RoutedEventArgs e)
        {
            if (appendTextBox.Text.Length == 0)
                return;

            AppendToFile(appendTextBox.Text);
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            fileContentsTextBox.Text = await GetAppendFileAsync();
        }

        private async void AppendToFile(string textToAppend)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(textToAppend);
            try
            {
                StorageFile appendFile = await localFolder.CreateFileAsync("append.txt", CreationCollisionOption.OpenIfExists);
                await FileIO.AppendTextAsync(appendFile, sb.ToString());
            }
            catch (Exception e)
            {
                await new MessageDialog("error occurred when appending text: " + e.Message, "uh oh spaghettio").ShowAsync();
            }
        }

        private async Task<string> GetAppendFileAsync()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile appendFile = await localFolder.GetFileAsync("append.txt");
                return await FileIO.ReadTextAsync(appendFile);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            //fileContentsTextBox.SelectAll();
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(fileContentsTextBox.Text);
            Clipboard.SetContent(dataPackage);
        }
    }
}
