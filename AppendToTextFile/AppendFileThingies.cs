using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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
                await new MessageDialog("error occurred when appending text: " + e.Message, "uh oh spaghettio").ShowAsync();
            }
        }

        public async Task<string> GetAppendFileAsync()
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
    }
}
