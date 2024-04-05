using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace AppendToTextFile
{
    public class AppendFileThingies
    {
        public async Task AppendToFile(string textToAppend)
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
                SendToast("error occurred when appending text: " + e.Message);
            }
        }

        public async Task<string> GetAppendFileAsync()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile appendFile = await localFolder.GetFileAsync("append.txt");
                return await GetFileAsync(appendFile);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> GetFileAsync(StorageFile file)
        {
            try
            {
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task deleteFileAsync()
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile appendFile = await localFolder.GetFileAsync("append.txt");
                await appendFile.DeleteAsync();
            }
            catch (Exception e)
            {
                SendToast("error occurred when deleting file: " + e.Message);
            }
        }

        /// <summary>
        /// Simple method to show a basic toast with a message.
        /// </summary>
        /// <param name="message"></param>
        public void SendToast(string message)
        {
            ToastContent content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = message
                            }
                        }
                    }
                },

                //Audio = new ToastAudio()
                //{
                //    Src = new Uri(sound)
                //}
            };

            ToastNotification toast = new ToastNotification(content.GetXml());
            //toast.ExpirationTime = DateTime.Now.AddSeconds(600);

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
