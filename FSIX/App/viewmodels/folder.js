define(['services/dataservice', 'services/logger'], function (dataservice, logger) {
    var vm = {
        activate: activate,
        title: 'Folder',
        items: ko.observableArray()
    };
    return vm;

    //#region Internal Methods
    function activate(routeData) {
        logger.log('Folder View Activated', null, 'folder', false);
        getFolderDetails(routeData.id);
        return true;
    }

    function getFolderDetails(id) {
        dataservice.getFolderDetails(id)
            .then(querySucceeded)
            .fail(queryFailed);
    }

    function querySucceeded(data) {
        vm.items([]);
        data.results.forEach(function (item) {
            //extendItem(item);
            vm.items.push(item);
        });
        logger.info("Fetched Items");
    }

    function queryFailed(error) {
        logger.error(error.message, "Query failed");
    }

    //#endregion
});
