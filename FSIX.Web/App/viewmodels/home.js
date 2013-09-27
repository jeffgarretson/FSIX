// Home ViewModel
define(['dataservice', 'logger'], function (dataservice, logger) {

    vm = {
        displayName: "Home",
        folders: ko.observableArray(),
        error: ko.observable(),
        activate: activate,
        getFolderDetails: getFolderDetails
    };

    return vm;

    function activate() {
        //getFolderDetails(parseInt(folderId));
    }

    //#region Private functions

    function getFolderDetails(id) {
        dataservice.getFolderDetails(id)
            .then(querySucceeded)
            .fail(queryFailed)
            .fin(refreshView);
    }

    function querySucceeded(data) {
        vm.folders(data);
        logger.log("Fetched items", data, "home.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "home.js", false);
    }

    //#endregion

});
