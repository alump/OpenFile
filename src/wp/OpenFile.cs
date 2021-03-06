/*
 * Copyright (C) 2015 Sami Viitanen <sami.viitanen@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
 
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;
using Windows.Foundation;
 
namespace WPCordovaClassLib.Cordova.Commands {
 
    public class OpenFile : BaseCommand {
 
        private string inputUri;
 
        private void ErrorResponse(string message)
        {
            DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, message));
        }
 
        public void LaunchCallback(IAsyncOperation<bool> sender, AsyncStatus asyncStatus)
        {
            if (asyncStatus == AsyncStatus.Completed)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK));
            }
            else if (asyncStatus == AsyncStatus.Error)
            {
                ErrorResponse("Error while launching application for file");
            }
            else if (asyncStatus == AsyncStatus.Canceled)
            {
                ErrorResponse("Launching application for file cancelled");
            }
        }
 
        public void FileCallback(IAsyncOperation<StorageFile> sender, Windows.Foundation.AsyncStatus asyncStatus)
        {
            if (asyncStatus == Windows.Foundation.AsyncStatus.Completed)
            {
                try
                {
                    StorageFile file = sender.GetResults();
                    IAsyncOperation<bool> op = Windows.System.Launcher.LaunchFileAsync(file);
                    if (op.Status == AsyncStatus.Completed)
                    {
                        LaunchCallback(op, op.Status);
                    }
                    else
                    {
                        op.Completed = new Windows.Foundation.AsyncOperationCompletedHandler<bool>(LaunchCallback);
                    }
                }
                catch (System.Exception e)
                {
                    ErrorResponse("Exception: " + e.ToString());
                }
            }
            else if (asyncStatus == AsyncStatus.Canceled)
            {
                ErrorResponse("File locating canceled");
            }
            else if (asyncStatus == AsyncStatus.Error)
            {
                ErrorResponse("Failed to find file '" + inputUri + "'");
            }
        }
 
        public void open(string jsonInput) {
            string optVal = null;
 
            try {
                optVal = JsonHelper.Deserialize<string[]>(jsonInput)[0];
            } catch(System.Exception) {}
 
            if(optVal == null) {
               DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION));
            }
 
            string filePath = optVal.Replace('/', '\\');
            inputUri = filePath;
 
            try {
                    StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Foundation.IAsyncOperation<StorageFile> op = local.GetFileAsync(filePath);
                    if (op.Status == AsyncStatus.Completed)
                    {
                        FileCallback(op, op.Status);
                    }
                    else
                    {
                        op.Completed = new Windows.Foundation.AsyncOperationCompletedHandler<StorageFile>(FileCallback);
                    }
            } 
            catch(System.IO.FileNotFoundException)
            {
                ErrorResponse("File not found " + inputUri);
            }
            catch (System.UnauthorizedAccessException)
            {
                ErrorResponse("Unauthorized Access " + inputUri);
            }
            catch(System.Exception e)
            {
                ErrorResponse("Exception " + e.ToString());
            }
        }
    }
}