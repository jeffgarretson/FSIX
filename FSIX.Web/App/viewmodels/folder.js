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
        expireFolder: expireFolder,
        editFolderDetails: editFolderDetails,
        cancelEditFolderDetails: cancelEditFolderDetails,
        updateFolderDetails: updateFolderDetails,
        editFolderDetailsFormVisible: ko.observable(false)
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
        logger.success("Fetched items", data, "folder.js", false);
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
        var self = this;
        app.showMessage("Expire this folder and PERMANENTLY DELETE all contents?", "Expire folder", ["Yes", "No"])
            .then(function (response) {
                if ("Yes" == response) {
                    self.expire()
                        .then(router.navigateBack())
                        .then(logger.success('Folder "' + self.name() + '" expired', null, "folder.js", true));
                }
            });
    };

    function editFolderDetails() {
        vmFolder.editFolderDetailsFormVisible(true);
    }

    function cancelEditFolderDetails() {
        var self = this;
        if (self.entityAspect.entityState.isModified()) {
            app.showMessage("Cancel changes?", "Cancel", ["Yes", "No"])
                .then(function (response) {
                    if ("Yes" == response) { cancel(); }
                });
        } else {
            cancel();
        }

        function cancel() {
            self.entityAspect.rejectChanges();
            vmFolder.editFolderDetailsFormVisible(false);
        }
    }

    function updateFolderDetails() {
        var self = this;
        dataservice.saveEntity(self)
            .then(success)
            .fail(fail);
        function success() {
            vmFolder.editFolderDetailsFormVisible(false);
            logger.success("Updated folder \"" + self.name() + "\"", null, null, true);
        }
        function fail() {
            self.entityAspect.rejectChanges();
            logger.error("Failed to update folder \"" + self.name() + "\"", null, null, true);
        }
    }

    //#endregion

});
