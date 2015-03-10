/*  
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using WPCordovaClassLib.Cordova;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;

namespace WPCordovaClassLib.Cordova.Commands {

    public class OpenFile : BaseCommand {

    	private static final int 

    	public void open(string jsonInput) {
            string optVal = null;

            try {
                optVal = JsonHelper.Deserialize<string[]>(jsonInput)[0];
            } catch(Exception) {}

            if(optVal == null) {
               DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION));
            }

            string filePath = optVal.Replace('/', '\\').Substring(2);

    		try {
    			StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
    			StorageFile file = await folder.GetFileAsync(filePath);

    			if(file == null) {
    				DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "File '" + filePath + "' not found"));
    				return;
    			}

    			var launchOk = await Windows.System.Launcher.LaunchFileAsync(file);
    			if(launchOk) {
    				DispatchCommandResult(new PluginResult(PluginResult.Status.OK));
    			} else {
					DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Failed to launch application for file '" + filePath + "'"));
    			}
    		} catch(Exception e) {
    			DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Exception: " + e.Message));
    		}
    	}
    }
}
