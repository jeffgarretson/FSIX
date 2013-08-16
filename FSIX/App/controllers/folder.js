fsix.controller('FolderCtrl',
    ['$scope', '$routeParams', 'dataservice', 'logger',
    function ($scope, $routeParams, dataservice, logger) {

        logger.log("Creating Folder Details Controller", null, "folder.js", false);

        $scope.title = "Folder Details";
        $scope.folders = [];
        $scope.error = "";
        $scope.getFolderDetails = getFolderDetails;

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
            console.log("XHR success");
        }

        function queryFailed(error) {
            $scope.error = error.message;
            console.log("XHR failure");
        }

        function refreshView() { $scope.$apply(); }

        //#endregion

    }]);
