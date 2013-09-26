define(['durandal/app', 'moment', 'dataservice', 'logger'], function (app, moment, dataservice, logger) {

    vm = {
        activate: activate,
        displayName: "Folders",
        folders: ko.observableArray(),
        error: "",
        getFolders: getFolders,
        showNewFolderForm: showNewFolderForm,
        shouldShowNewFolderForm: ko.observable(false),
        newFolder: ko.observable(),
        addFolder: addFolder,
        endEdit: endEdit
    };

    return vm;

    //#region Private functions

    function activate() {
        getFolders();
    }

    function getFolders() {
        dataservice.getFolders()
            .then(querySucceeded)
            .fail(queryFailed)
            .fin(refreshView);
    }

    function querySucceeded(data) {
        vm.folders(data);
        logger.log("Fetched items", data, "folders.js", false);
    }

    function queryFailed(error) {
        logger.error("Failed to get items", error, "folders.js", false);
    }

    function refreshView() {
        console.log("Wow, what a refreshing view!");
    }

    function createNewFolderTemplate() {
        return dataservice.createFolder();
    }

    function showNewFolderForm() {
        if (undefined === vm.newFolder()) {
            vm.newFolder = createNewFolderTemplate();
        }
        vm.shouldShowNewFolderForm(true);
    }

    function addFolder() {
        var folder = dataservice.createFolder({
            name: vm.newFolder.name(),
            description: vm.newFolder.description(),
            expirationDate: new moment().add('days', 14)
        });
        var permission = dataservice.createPermission({
            username: "JeffGarretson",
            isOwner: true,
            permRead: true,
            permWrite: true,
            permShare: true
        })
        permission.folder(folder);
        dataservice.saveChanges().fail(addFailed);
        vm.folders.push(folder);   // .unshift()?

        vm.newFolder.name("");
        vm.newFolder.description("");
        vm.shouldShowNewFolderForm(false);

        function addFailed() {
            var index = vm.folders.indexOf(folder);
            if (index > -1) {
                setTimeout(function () { vm.folders.splice(index, 1); }, 2000);
            }
        }

    }

    function endEdit(entity) {
        dataservice.saveEntity(entity).fin(refreshView);
    }

    //#endregion
});

