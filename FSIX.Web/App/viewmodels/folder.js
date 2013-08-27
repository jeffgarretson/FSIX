// Folder ViewModel
define(['dataservice', 'logger'], function (dataservice, logger) {

    vm = {
        displayName: "Folder Details",
        folders: ko.observableArray(),
        error: ko.observable(),
        activate: activate,
        getFolderDetails: getFolderDetails,
        newItemType: ko.observable("Note"),
        addNote: addNote,
        addFile: addFile,
        addImage: addImage,
        managePermissions: managePermissions
    };

    return vm;

    function activate(folderId) {
        getFolderDetails(parseInt(folderId));
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
        logger.log("Fetched items", data, "folder.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "folder.js", false);
    }

    function endEdit(entity) {
        dataservice.saveEntity(entity).fin(refreshView);
    }

    function addNote() {
        var item = dataservice.createItem();
        // How do I refer to the current folder? Do I need to pass it in from the ng-click event?
        // item.name =
        dataservice.saveEntity(item)
            .then(addSucceeded)
            .fail(addFailed)
            .fin(refreshView);

        function addSucceeded() {
            showAddedItem(item);
        }

        function addFailed(error) {
            failed({ message: "Save of new item failed" });
        }
    }

    function addFile() {
        alert("User wants to add a new FILE. Double-Ha!!!");
    }

    function addImage() {
        alert("Add a new IMAGE? Just try it!");
    }

    function managePermissions() {
        alert("You wanna manage PERMISSIONS? Man, you craaaaazy!");
    }

    function showAddedItem(item) {
        vm.folders.items.unshift(item);
    }

    function refreshView() { /* $scope.$apply(); */ }

    //#endregion

});
