OpenFile Cordova Plug-In
========================

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

Authors
-------
Windows support is written by Doan Isakov doan.isakov(at)gmail.com, he has also provided improvements to
initial iOS implementation.

Other parts are written by Sami Viitanen sami.viitanen(at)gmail.com

Copyrights
----------
Copyrights belong to people listed in files. Files without copyright information are copyrighted by Sami Viitanen
<sami.viitanen@gmail.com>

Supported Platforms
-------------------
Currently Android, iOS, Windows Phone 8 and Windows (8+).

I'm more than happy to add other platforms if patches are provided with license compatible with Apache 2.0.

Installation
------------

Add plug-in to your PhoneGap/Cordova project with:

`phonegap plugin add org.vaadin.alump.openfile`

or

`cordova plugin add org.vaadin.alump.openfile`

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

Release History
---------------
### 0.2.0 (2015-05-09)
  * Improvements to iOS implementation. Thanks to Doan Isakov!
  * Windows 8 (universal) support added. Kudos to Doan Isakov!

### 0.1.0 (2015-03-13)
  * Initial release
  * Android, iOS and Windows Phone 8 support

Planned features
----------------
 * Proper error codes
 * Adding new support to new platforms