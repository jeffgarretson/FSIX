define(['services/dataservice', 'services/logger'], function (dataservice, logger) {
    var vm = {
        activate: activate,
        title: 'Active Folders',
        items: ko.observableArray()
    };
    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Active Folders View Activated', null, 'active', false);
        getFolders();
        return true;
    }

    function getFolders() {
        dataservice.getFolders()
            .then(querySucceeded)
            .fail(queryFailed);
    }

    function querySucceeded(data) {
        vm.items([]);
        data.results.forEach(function (item) {
            extendItem(item);
            vm.items.push(item);
        });
        logger.info("Fetched Items");
    }

    function queryFailed(error) {
        logger.error(error.message, "Query failed");
    }

    function extendItem(item) {
        item.Url = ko.computed(function () {
            return '#/folder/' + this.Id();
        }, item);
    }

    //#endregion
});