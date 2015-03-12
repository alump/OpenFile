package org.vaadin.alump.openfile;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.util.Log;
import android.webkit.MimeTypeMap;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaPlugin;

import org.json.JSONArray;
import org.json.JSONException;

/**
 * This class starts an activity for an intent to view files
 */
public class OpenFile extends CordovaPlugin {

  private static final String OPEN_COMMAND = "open";
  private static final String TAG = OpenFile.class.getSimpleName();

  @Override
  public boolean execute(String command, JSONArray jsonArgs, CallbackContext callback) throws JSONException {
    if(OPEN_COMMAND.equals(command)) {
      final String uri = jsonArgs.getString(0);
      findIntent(uri, callback);
    } else {
      Log.w(TAG, "Unknown command '" + command + "' ignored.");
      return false;
    }

    return true;
  }

  private void findIntent(String uri, CallbackContext callback) {
    if(uri == null || uri.isEmpty()) {
      final String error = "Invalid path " + (uri == null ? "null" : ("'" + uri + "'")) + " received.";
      Log.e(TAG, error);
      callback.error(error);
      return;
    }

    try {
        Uri realUri = Uri.parse(uri);
        String mime = getMimeTypeFromExtension(uri);
        Intent fileIntent = new Intent(Intent.ACTION_VIEW);

        if(Build.VERSION.SDK_INT >= 16) {
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
    if(extension != null) {
      mimeType = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension);
      Log.d(TAG, "Mime type resolved as " + mimeType);
    } else {
      Log.w(TAG, "Failed to resolve extension from " + uri);
    }

    return mimeType;
  }
}
