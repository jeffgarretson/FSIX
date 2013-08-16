fsix.controller('NewCtrl',
    ['$scope', 'dataservice', 'logger',
    function ($scope, dataservice, logger) {

        logger.log("Creating New Folder Controller", null, "new.js", false);

        $scope.title = "Create New Folder";
        $scope.folders = [];
        $scope.error = "";
        $scope.getFolders = getFolders;
        $scope.refresh = refresh;

        // Load folder list immediately (from cache if possible)
        $scope.getFolders();

        //#region Private functions
        function getFolders(forceRefresh) {
            dataservice.getFolders(forceRefresh)
                .then(querySucceeded)
                .fail(queryFailed)
                .fin(refreshView);
        }

        function refresh() { getFolders(true); }

        function querySucceeded(data) {
            $scope.folders = data;
            logger.info("Fetched Items");
        }

        function queryFailed(error) {
            $scope.error = error.message;
        }

        function refreshView() { $scope.$apply(); }

        //#endregion
    }]);
