fsix.controller('AdminCtrl',
    ['$scope', 'dataservice', 'logger',
    function ($scope, dataservice, logger) {

        logger.log("Creating Admin Controller", null, "admin.js", false);

        $scope.title = "Admin";
        $scope.folders = [];
        $scope.error = "";

        //#region Private functions

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
