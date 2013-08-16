fsix.controller('FolderCtrl',
    ['$scope', '$routeParams', 'dataservice', 'logger',
    function ($scope, $routeParams, dataservice, logger) {

        logger.log("Creating Folder Details Controller", null, "folder.js", false);

        $scope.title = "Folder Details";
        $scope.folders = [];
        $scope.error = "";
        $scope.getFolderDetails = getFolderDetails;
        $scope.newItem = newItem;

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

        function newItem() {
            alert("User wants to add a new item. Ha!!!");
        }

        function refreshView() { $scope.$apply(); }

        //#endregion

    }]);
