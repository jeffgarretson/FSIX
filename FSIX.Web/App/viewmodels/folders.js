define(
    ['plugins/router', 'durandal/app', 'moment', 'dataservice', 'logger'],
    function (router, app, moment, dataservice, logger) {
        "use strict";

    var vmFolders = {
        activate: activate,
        displayName: "Folders",
        folders: ko.observableArray(),
        error: "",
        getFolders: getFolders,
        showNewFolderForm: showNewFolderForm,
        newFolderFormVisible: ko.observable(false),
        newFolder: ko.observable(),
        addFolder: testAddFolder,
        createFolder: testAddFolder,
        endEdit: endEdit,
        hideNewFolderForm: hideNewFolderForm
    };

    return vmFolders;

    //#region Private functions

    function activate() {
        //logger.log("Activate()", null, "folders.js", true);
        //vm.newFolderFormVisible(false);
        getFolders();
    }

    function getFolders() {
        dataservice.getFolders()
            .then(querySucceeded)
            .fail(queryFailed);
    }

    function querySucceeded(data) {
        vmFolders.folders(data);
        logger.log("Fetched items", data, "folders.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "folders.js", false);
    }

    function refreshView() {}

    //function createNewFolderTemplate() {
    //    return dataservice.createFolder();
    //}

    function showNewFolderForm() {
        vmFolders.newFolder = dataservice.createFolder();
        vmFolders.newFolderFormVisible(true);
    }

    function hideNewFolderForm() {
        // Check whether newFolder is modified and, if so, prompt user before canceling?
        vmFolders.newFolderFormVisible(false);
        vmFolders.newFolder.entityAspect.setDeleted();
        vmFolders.newFolder = ko.observable();  // reset
    }



    function testAddFolder() {
        // Set initial expiration for 14 days
        vmFolders.newFolder.expirationDate(new Date(moment().add('days', 14)));
        // Make myself the owner (TODO: don't hard-code the username)
        vmFolders.newFolder.permissions.push(dataservice.createPermission({
            username: "JeffGarretson",
            isOwner: true,
            permRead: true,
            permWrite: true,
            permShare: true,
            folder: vmFolders.newFolder
        }));
        // Save changes
        dataservice.saveChanges()
            .then(addSucceeded)
            .fail(addFailed);
        function addSucceeded() {
            logger.log("Created folder " + vmFolders.newFolder.name(), null, null, true);
            vmFolders.newFolderFormVisible(false);
            vmFolders.folders.unshift(vmFolders.newFolder);
            router.navigate(vmFolders.newFolder.url);
            vmFolders.newFolder = ko.observable();  // reset
        }
        function addFailed() {
            logger.error("Folder add failed", null, null, true);
            var index = vmFolders.folders.indexOf(vmFolders.newFolder);
            if (index > -1) {
                setTimeout(function () { vmFolders.folders.splice(index, 1); }, 2000);
            }
        }
    }





    function addFolder() {
        //var folder = dataservice.createFolder({
        //    name: vmFolders.newFolder.name(),
        //    description: vmFolders.newFolder.description(),
        //    expirationDate: new Date(moment().add('days', 14))
        //});
        vmFolders.newFolder.expirationDate = ko.observable(new Date(moment().add('days', 14)));
        var permission = dataservice.createPermission({
            username: "JeffGarretson",
            isOwner: true,
            permRead: true,
            permWrite: true,
            permShare: true,
            folder: vmFolders.newFolder
        });
        //permission.folder(folder);
        dataservice.saveChanges()
            .then(addSucceeded)
            .fail(addFailed);

        //vmFolders.newFolder.name("");
        //vmFolders.newFolder.description("");
        vmFolders.newFolderFormVisible(false);

        function addSucceeded(saveResult) {
            vmFolders.folders.unshift(saveResult.entities);
            router.navigate(saveResult.entities[1].url);
        }

        function addFailed() {
            var index = vmFolders.folders.indexOf(vmFolders.newFolder);
            if (index > -1) {
                setTimeout(function () { vmFolders.folders.splice(index, 1); }, 2000);
            }
        }

    }

    function createFolder() {
    }

    function endEdit(entity) {
        dataservice.saveEntity(entity).fin(refreshView);
    }

    //function canDeactivate() {
    //    if (vmFolders.newFolderFormVisible()) {
    //        return app.showMessage("Leave this page and cancel new folder?", "Navigate", ["Yes", "No"]);
    //    } else {
    //        return true;
    //    }
    //}

    //#endregion
});

