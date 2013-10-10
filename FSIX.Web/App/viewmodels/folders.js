define(
    ['plugins/router', 'durandal/app', 'moment', 'dataservice', 'logger'],
    function (router, app, moment, dataservice, logger) {
        "use strict";

        var vmFolders = {
            activate: activate,
            displayName: "Folders",
            folders: ko.observableArray(),
            getFolders: getFolders,
            newFolderFormVisible: ko.observable(false),
            showNewFolderForm: showNewFolderForm,
            hideNewFolderForm: hideNewFolderForm,
            newFolder: ko.observable(),
            createFolder: createFolder
        };

        return vmFolders;

        function activate() {
            vmFolders.newFolderFormVisible(false);
            getFolders();
        }

        function getFolders() {
            dataservice.getFolders()
                .then(querySucceeded)
                .fail(queryFailed);

            function querySucceeded(data) {
                vmFolders.folders(data);
                logger.success("Fetched folder data", data, "folders.js", false);
            }

            function queryFailed(error) {
                logger.error("Failed to get folder data", error, "folders.js", true);
            }

        }

        function showNewFolderForm() {
            vmFolders.newFolder = dataservice.createFolder();
            vmFolders.newFolderFormVisible(true);
        }

        function hideNewFolderForm() {
            // TODO: check whether newFolder is modified and, if so, prompt user before canceling
            vmFolders.newFolderFormVisible(false);
            vmFolders.newFolder.entityAspect.setDeleted();
            vmFolders.newFolder = ko.observable();  // reset
        }

        function createFolder() {
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
                vmFolders.newFolderFormVisible(false);
                //vmFolders.folders.unshift(vmFolders.newFolder);
                router.navigate(vmFolders.newFolder.url());
                vmFolders.newFolder = ko.observable();  // reset
                logger.success("Created folder \"" + vmFolders.newFolder.name() + "\"", null, null, true);
            }
            function addFailed() {
                logger.error("Failed to add folder \"" + vmFolders.newFolder.name() + "\"", null, null, true);
                var index = vmFolders.folders.indexOf(vmFolders.newFolder);
                if (index > -1) {
                    setTimeout(function () { vmFolders.folders.splice(index, 1); }, 2000);
                }
            }
        }

    });

