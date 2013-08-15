define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: 'Expired Folders'
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Expired Folders View Activated', null, 'expired', false);
        return true;
    }
    //#endregion
});