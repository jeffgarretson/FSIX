fsix.controller('FolderCtrl',
    ['$scope', '$routeParams', 'dataservice', 'logger',
    function ($scope, $routeParams, dataservice, logger) {

        logger.log("Creating Folder Details Controller", null, "folder.js", false);

        angular.extend($scope, {
            title: "Folder Details",
            folders: [],
            error: "",
            getFolderDetails: getFolderDetails,
            addNote: addNote,
            addFile: addFile,
            managePermissions: managePermissions
        });

        // Load folder details immediately
        $scope.getFolderDetails(parseInt($routeParams.id));

        //#region Private functions
        function getFolderDetails(id) {
            dataservice.getFolderDetails(id)
                .then(querySucceeded)
                .fail(queryFailed)
                .fin(refreshView);
        }

        function querySucceeded(data) {
            $scope.folders = data;
            logger.info("Fetched Items");
        }

        function queryFailed(error) {
            $scope.error = error.message;
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

        function managePermissions() {
            alert("You wanna manage PERMISSIONS? Man, you craaaaazy!");
        }

        function showAddedItem(item) {
            $scope.folders.items.unshift(item);
        }

        function refreshView() { $scope.$apply(); }

        //#endregion

    }]);
