fsix.controller('FoldersCtrl',
    ['$scope', 'dataservice', 'logger',
    function ($scope, dataservice, logger) {

        logger.log("Creating Folders Controller", null, "folders.js", false);

        angular.extend($scope, {
            title : "Folders",
            folders : [],
            error : "",
            getFolders : getFolders,
            refresh : refresh,
            addFolder : addFolder
        });

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

        function addFolder() {
            alert("User wants to add a folder. Seems like a reasonable request.");
        }

        //#endregion
    }]);
