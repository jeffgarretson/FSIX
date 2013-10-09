// Home ViewModel
define(['dataservice', 'logger'], function (dataservice, logger) {
    "use strict";

    var vmHome = {
        displayName: "Home",
        folders: ko.observableArray(),
        error: ko.observable(),
        activate: activate,
        getFolderDetails: getFolderDetails
    };

    return vmHome;

    function activate() {
        //getFolderDetails(parseInt(folderId));
    }

    function getFolderDetails(id) {
        dataservice.getFolderDetails(id)
            .then(querySucceeded)
            .fail(queryFailed)
            .fin(refreshView);
    }

    function querySucceeded(data) {
        vmHome.folders(data);
        logger.log("Fetched items", data, "home.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "home.js", false);
    }

});
