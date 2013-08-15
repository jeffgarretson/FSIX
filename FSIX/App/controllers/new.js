define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: 'New Folder'
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('New Folder View Activated', null, 'new', false);
        return true;
    }
    //#endregion
});