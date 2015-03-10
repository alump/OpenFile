// TODO

module.exports = function (uri, success_cb, failure_cb) {
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

	console.log("OpenFile: Open '" + uri + "'");
	cordova.exec(success_cb, failure_cb, "OpenFile", "open", [uri]);
}