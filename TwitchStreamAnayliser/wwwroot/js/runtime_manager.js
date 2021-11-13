function calculateStreamRuntime(start, end) {
	var output;
	var elapsed = end - start;

	var seconds = Math.floor(elapsed / 1000);
	var minutes = Math.floor(seconds / 60);
	var hours = Math.floor(minutes / 60);
	var days = Math.floor(hours / 24);

	if (days >= 1) {
		output = '' + days + ':' + (hours % 24) + ':' + formatNumberLength(minutes % 60, 2) + ':' + formatNumberLength(seconds % 60, 2)
	}
	else {
		output = '' + hours + ':' + formatNumberLength(minutes % 60, 2) + ':' + formatNumberLength(seconds % 60, 2)
	}

	return output;
}
function formatNumberLength(num, length) {
	var r = '' + num;
	while (r.length < length) {
		r = '0' + r;
	}

	return r;
}

function updateStreamRuntime(startDatetime) {
	$('#runtime').text(calculateStreamRuntime(startDatetime, Date.now()));
}