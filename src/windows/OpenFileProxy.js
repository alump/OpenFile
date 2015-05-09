/*
 * Copyright (C) 2015 Doan Isakov <doan.isakov@gmail.com>
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

/** Plugin definition with methods to locate and launch a file */
var OpenFileProxy = {
    getWinFolder: function (entry) {
        var fsType = entry.filesystem.name;
        var appData = Windows.Storage.ApplicationData.current;
        if (fsType === 'persistent') {
            return appData.localFolder;
        } else if (fsType === 'temporary') {
            return appData.temporaryFolder;
        }
        return null;
    },
    ensurePathRelative: function (entry) {
        var filePath = entry.fullPath;
        if (filePath.indexOf("/") == 0) {
            filePath = filePath.slice(1);
        }
        return filePath;
    },
    doLaunch: function (file) {
        Windows.System.Launcher.launchFileAsync(file).then(
            function (success) {
                if (success) {
                    OpenFileProxy.success_cb();
                } else {
                    OpenFileProxy.failure_cb({
                        code: -2,
                        description: 'File successfully located, but launch failed!'
                    });
                }
            }
        );
    },
    resolveLaunch: function (fileUrl) {
        window.resolveLocalFileSystemURL(fileUrl,
            function (entry) {
                var filePath = OpenFileProxy.ensurePathRelative(entry);
                var winFolder = OpenFileProxy.getWinFolder(entry);
                if (winFolder) {
                    winFolder.getFileAsync(filePath).then(
                        OpenFileProxy.doLaunch,
                        function (error) {
                            var cordovaFileError = {
                                code: FileError.NOT_FOUND_ERR,
                                winError: error
                            };
                            OpenFileProxy.failure_cb(cordovaFileError);
                        }
                    );
                } else {
                    OpenFileProxy.failure_cb({
                        code: FileError.NOT_FOUND_ERR,
                        description: 'Unknown file system specified.'
                    });
                }
            },
            function (error) {
                OpenFileProxy.failure_cb(error);
            }
        );
    },
    success_cb: null,
    failure_cb: null,
    /**
     * Opens files on Windows 8 and Windows 8.1.
     * @param onSuccess success callback
     * @param onFailure failure callback
     * @param callArgs an array of argument for proxy call
     */
    open: function (onSuccess, onFailure, callArgs) {
        if (callArgs.length > 0) {
            var fileUrl = callArgs[0];
            OpenFileProxy.success_cb = onSuccess;
            OpenFileProxy.failure_cb = onFailure;
            OpenFileProxy.resolveLaunch(fileUrl);
        } else {
            failure_cb({
                code: -1,
                description: 'Missing file URL!'
            });
        }
    }
};

/** Plugin registration */
cordova.commandProxy.add("OpenFile", OpenFileProxy);