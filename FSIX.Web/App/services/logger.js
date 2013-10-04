define(
    ['durandal/system', 'toastr'],
    function (system, toastr) {
        return {
            log: info,
            info: info,
            success: success,
            warning: warning,
            error: error
        };

        function info(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'info');
        }

        function success(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'success');
        }

        function warning(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'warning');
        }

        function error(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'error');
        }

        function logIt(message, data, source, showToast, toastType) {
            source = source ? '[' + source + '] ' : '';
            if (data) {
                system.log(source, message, data);
            } else {
                system.log(source, message);
            }
            if (showToast) {
                switch (toastType) {
                    case "error":
                        toastr.error(message);
                        break;
                    case "warning":
                        toastr.warning(message);
                        break;
                    case "success":
                        toastr.success(message);
                        break;
                    default:
                        toastr.info(message);
                }
            }
        }
    }
);
