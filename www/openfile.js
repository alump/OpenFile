/*
 * Copyright (C) 2015 Sami Viitanen <sami.viitanen@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

module.exports = {
	/**
	 * Open file
	 * @param {String}   uri        File URI
	 * @param {Function} success_cb Callback called when file openned successfully
	 * @param {Function} failure_cb Callback called if open failes with error message parameter
	 */
	open: function (uri, success_cb, failure_cb) {
		if(typeof success_cb == 'undefined') {
			success_cb = function() {
				console.log("OpenFile: Success");
			}
		}
		if(typeof failure_cb == 'undefined') {
			failure_cb = function(error) {
				console.log("OpenFile: Failure '" + error + "'");
				alert(error);
			}
		}

		//alert("Open '" + uri + "' requested");
		console.log("OpenFile: Open '" + uri + "'");
		cordova.exec(success_cb, failure_cb, "OpenFile", "open", [uri]);
	}
};