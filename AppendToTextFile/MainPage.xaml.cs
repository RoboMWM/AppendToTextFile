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
        AppendFileThingies thingies;
        public MainPage()
        {
            thingies = new AppendFileThingies();
            this.InitializeComponent();
        }

        private async void AppendButton_Click(object sender, RoutedEventArgs e)
        {
            if (appendTextBox.Text.Length == 0)
                return;

            await thingies.AppendToFile(appendTextBox.Text);
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            fileContentsTextBox.Text = await thingies.GetAppendFileAsync();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            //fileContentsTextBox.SelectAll();
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(fileContentsTextBox.Text);
            Clipboard.SetContent(dataPackage);
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog deletePrompt = new MessageDialog("Delete the entire text file, clearing its contents? This is a destructive action.", "Delete text file");
            deletePrompt.Commands.Add(new UICommand("Delete", new UICommandInvokedHandler(this.deleteFileCommandHandler)));
            deletePrompt.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.deleteFileCommandHandler)));
            // Set the command that will be invoked by default
            deletePrompt.DefaultCommandIndex = 1;
            // Set the command to be invoked when escape is pressed
            deletePrompt.CancelCommandIndex = 1;

            await deletePrompt.ShowAsync();
        }

        private async void deleteFileCommandHandler(IUICommand command)
        {
            switch (command.Label)
            {
                case "Delete":
                    await thingies.deleteFileAsync();
                    break;
                case "Cancel":
                default:
                    return;
            }
        }
    }
}
