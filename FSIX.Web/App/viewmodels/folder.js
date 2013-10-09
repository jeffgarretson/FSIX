// Folder ViewModel
define(
    ['plugins/router', 'durandal/app', 'dataservice', 'logger', 'plupload'],
    function (router, app, dataservice, logger, plupload) {
        "use strict";

        var uploader;
        var vmFolder = {
            displayName: "Folder Details",
            folders: ko.observableArray(),
            activate: activate,
            getFolderDetails: getFolderDetails,

            newItem: ko.observable(),
            newItemFormVisible: ko.observable(false),
            showNewItemForm: showNewItemForm,
            hideNewItemForm: hideNewItemForm,
            addItem: addItem,

            managePermissions: managePermissions,

            expireFolder: expireFolder,

            editFolderDetailsFormVisible: ko.observable(false),
            editFolderDetails: editFolderDetails,
            cancelEditFolderDetails: cancelEditFolderDetails,
            updateFolderDetails: updateFolderDetails
        };

        return vmFolder;

        function activate(folderId) {
            getFolderDetails(parseInt(folderId));
        }

        function getFolderDetails(id) {
            dataservice.getFolderDetails(id)
                .then(querySucceeded)
                .fail(queryFailed);

            function querySucceeded(data) {
                vmFolder.folders(data);
                logger.success("Fetched items", data, "folder.js", false);
            }

            function queryFailed(error) {
                logger.error("Failed to get items", error, "folder.js", false);
            }

        }

        function showNewItemForm() {
            vmFolder.newItem = dataservice.createItem();
            vmFolder.newItemFormVisible(true);
            uploader = new plupload.Uploader({
                runtimes: "html5,silverlight,flash,html4",
                browse_button: "upload-pickfiles",
                container: "upload-container",
                _url: ko.computed(function () { return "/api/media/" + vmFolder.newItem.id; }),
                silverlight_xap_url: "../Scripts/plupload/plupload.silverlight.xap",
                flash_swf_url: "../Scripts/plupload/plupload.flash.swf"
            });
            uploader.init();
        }

        function hideNewItemForm() {
            // TODO: check whether newItem is modified and, if so, prompt user before canceling
            uploader = null;
            vmFolder.newItemFormVisible(false);
            vmFolder.newItem.entityAspect.setDeleted();
            vmFolder.newItem = ko.observable();  // reset
        }

        function addItem() {
            var self = this;
            // Connect new item to this folder
            this.items.push(vmFolder.newItem);
            // Set properties
            vmFolder.newItem.createdTime(new Date());
            vmFolder.newItem.modifiedTime(new Date());
            vmFolder.newItem.createdBy(dataservice.currentUser());
            vmFolder.newItem.type("Note");
            // Save changes
            dataservice.saveChanges()
                .then(uploadFiles)
                .then(success)
                .fail(failure);
            function uploadFiles() {
                if (uploader.files.length) {
                    uploader.settings.url = vmFolder.newItem.fileUploadUrl();
                    uploader.start();
                }
            }
            function success() {
                vmFolder.newItemFormVisible(false);
                vmFolder.newItem = ko.observable();
                logger.success("New item saved successfully", null, null, true);
            }
            function failure() {
                logger.error("Failed to save new item", null, null, true);
                // Show the failed item for a bit, then remove it
                var index = self.items.indexOf(vmFolder.newItem);
                if (index > -1) {
                    setTimeout(function () { self.items.splice(index, 1); }, 2000);
                }
            }
        }

        function managePermissions() {
            alert("You wanna manage PERMISSIONS? Man, you craaaaazy!");
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

    });
