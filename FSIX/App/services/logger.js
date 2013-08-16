fsix.factory('logger', function () {
    var logger = {
        log: log,
        logError: logError
    };

    return logger;

    function log(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'info');
    }

    function logError(message, data, source, showToast) {
        logIt(message, data, source, showToast, 'error');
    }

    function logIt(message, data, source, showToast, toastType) {
        //source = source ? '[' + source + '] ' : '';
        //if (data) {
        //    system.log(source, message, data);
        //} else {
        //    system.log(source, message);
        //}
        var console = window.console;
        !!console && console.log && console.log.apply && console.log.apply(console, arguments);
        if (showToast) {
            if (toastType === 'error') {
                toastr.error(message);
            } else {
                toastr.info(message);
            }

        }

    }
});
