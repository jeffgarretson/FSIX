﻿fsix.controller('ExpiredCtrl',
    ['$scope', 'dataservice', 'logger',
    function ($scope, dataservice, logger) {

        logger.log("Creating Expired Folders Controller", null, "expired.js", false);

        $scope.title = "Expired Folders";
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
