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

package org.vaadin.alump.openfile;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.util.Log;
import android.webkit.MimeTypeMap;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaWebView;

import org.json.JSONArray;
import org.json.JSONException;

/**
 * This class starts an activity for an intent to view files
 */
public class OpenFile extends CordovaPlugin {

  private static final String OPEN_ACTION = "open";
  private static final String TAG = OpenFile.class.getSimpleName();

  @Override
  public void initialize(CordovaInterface cordova, CordovaWebView webView) {
    super.initialize(cordova, webView);
    Log.d(TAG, "Initialized");
  }

  @Override
  public boolean execute(String action, JSONArray jsonArgs,
      CallbackContext callback) throws JSONException {
    if (OPEN_ACTION.equals(action)) {
      final String uri = jsonArgs.getString(0);
      findIntent(uri, callback);
    } else {
      Log.w(TAG, "Unknown command '" + action + "' ignored.");
      return false;
    }

    return true;
  }

  private void findIntent(String uri, CallbackContext callback) {
    if (uri == null || uri.isEmpty()) {
      final String error = "Invalid path "
          + (uri == null ? "null" : ("'" + uri + "'")) + " received.";
      Log.e(TAG, error);
      callback.error(error);
      return;
    }

    try {
      Uri realUri = Uri.parse(uri);
      String mime = getMimeTypeFromExtension(uri);
      Intent fileIntent = new Intent(Intent.ACTION_VIEW);

      if (Build.VERSION.SDK_INT >= 16) {
        fileIntent.setDataAndTypeAndNormalize(realUri, mime);
      } else {
        fileIntent.setDataAndType(realUri, mime);
      }

      Log.d(TAG, "Starting activity for '" + uri + "'...");
      cordova.getActivity().startActivity(fileIntent);

    } catch (ActivityNotFoundException e) {
      final String error = "Failed to find activity for '" + uri + "'";
      Log.e(TAG, error, e);
      callback.error(error + ": " + e.getMessage());
    }

    callback.success();
  }

  private String getMimeTypeFromExtension(String uri) {
    String mimeType = null;

    final String extension = MimeTypeMap.getFileExtensionFromUrl(uri);
    if (extension != null) {
      mimeType = MimeTypeMap.getSingleton().getMimeTypeFromExtension(
          extension);
      Log.d(TAG, "Mime type resolved as " + mimeType);
    } else {
      Log.w(TAG, "Failed to resolve extension from " + uri);
    }

    return mimeType;
  }
}
