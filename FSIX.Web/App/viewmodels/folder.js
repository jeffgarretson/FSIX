// Folder ViewModel
define(
    ['plugins/router', 'durandal/app', 'dataservice', 'logger'],
    function (router, app, dataservice, logger) {

    vmFolder = {
        displayName: "Folder Details",
        folders: ko.observableArray(),
        error: ko.observable(),
        activate: activate,
        getFolderDetails: getFolderDetails,
        newItemType: ko.observable("Note"),
        showNewItemForm: showNewItemForm,
        newItemFormVisible: ko.observable(false),
        postItem: postItem,
        cancelCreateItem: cancelCreateItem,
        addNote: addNote,
        addFile: addFile,
        addImage: addImage,
        managePermissions: managePermissions,
        expireFolder: expireFolder
    };

    return vmFolder;

    function activate(folderId) {
        getFolderDetails(parseInt(folderId));
    }

    //#region Private functions
    function getFolderDetails(id) {
        dataservice.getFolderDetails(id)
            .then(querySucceeded)
            .fail(queryFailed);
    }

    function querySucceeded(data) {
        vmFolder.folders(data);
        logger.log("Fetched items", data, "folder.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "folder.js", false);
    }

    function endEdit(entity) {
        dataservice.saveEntity(entity).fin(refreshView);
    }

    function showNewItemForm() {
        vmFolder.newItemFormVisible(true);
    }

    function postItem() { }

    function cancelCreateItem() {
        vmFolder.newItemFormVisible(false);
    }

    function addNote() {
        var item = dataservice.createItem();
        dataservice.saveEntity(item)
            .then(addSucceeded)
            .fail(addFailed);

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
        vmFolder.folders.items.unshift(item);
    }

    function expireFolder() {
        var that = this;
        app.showMessage("Expire this folder and PERMANENTLY DELETE all contents?", "Expire folder", ["Yes", "No"])
            .then(function (response) {
                if ("Yes" == response) {
                    that.expire()
                        .then(router.navigateBack())
                        .then(logger.log('Folder "' + that.name() + '" expired', null, "folder.js", true));
                }
            });
    };

    //#endregion

});
