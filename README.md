OpenFile Cordova Plug-In
========================

This plug-in is still under development!

What is it?
-----------
PhoneGap / Cordova plugin that adds possibility to open local files on other applications. Eg. open PDF-file you have
downloaded in device's PDF viewer.

This plug-in will be usually used with plug-in `com.apache.cordova.file-transfer` used to first load files to device,
to be then displayed with help of OpenFile plug-in.

What is the license?
--------------------
Plug-in is released with [Apache 2.0 license](LICENSE.txt). Allowing it to be used also in closed source commercial
projects.

Copyrights
----------
Copyrights belong to people listed in files. Files without copyright information are copyrighted by Sami Viitanen
<sami.viitanen@gmail.com>

Supported Platforms
-------------------
Currently Android, iOS and Windows Phone 8.

I'm more than happy to add other platforms if patches are provided with license compatible with Apache 2.0.

Installation
------------
Currently plug-in is not in Cordova plug-in repository. But if you have git available in your command line, you can
still install it with:

`phonegap plugin add https://github.com/alump/OpenFile.git`

or

`cordova plugin add https://github.com/alump/OpenFile.git`

Plan is to release plug-in in cordova plug-in repository soon, to make it easier to install it

How to use it
-------------
In JavaScript (easiest way):

    window.openfile.open(filePath);

Same from inside GWT JSNI:

    $wnd.openfile.open(filePath);

Full JavaScript example, with custom result callbacks defined:

    window.openfile.open(filePath,
         function() {
             console.log('File open successfully');
         },
         function(error) {
             console.log('Failed to open file: ' + error);
         }
    );

Planned features
----------------
 * Proper error codes
 * Adding new supported platforms