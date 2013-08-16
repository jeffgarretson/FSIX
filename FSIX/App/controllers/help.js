fsix.controller('HelpCtrl',
    ['$scope', 'dataservice', 'logger',
    function ($scope, dataservice, logger) {

        logger.log("Creating Help Controller", null, "help.js", false);

        $scope.title = "Help";
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
