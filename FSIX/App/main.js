﻿/* main: startup script creates the 'fsix' module and adds custom Ng directives */

// 'fsix' module is in global namespace
window.fsix = angular.module('fsix', []);

// Add global "services" (like breeze and Q) to the Ng injector
// Learn about Angular dependency injection in this video
// http://www.youtube.com/watch?feature=player_embedded&v=1CpiB3Wk25U#t=2253s
fsix.value('breeze', window.breeze)
    .value('Q', window.Q);


// Configure routes
fsix.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        //.when('/', { templateUrl: 'app/views/folders.html', controller: 'FoldersCtrl' })
        .when('/folders/', { templateUrl: 'app/views/folders.html', controller: 'FoldersCtrl' })
        .when('/folders/expired/', { templateUrl: 'app/views/folders.html', controller: 'FoldersCtrl' })
        .when('/folder/:id/', { templateUrl: 'app/views/folder.html', controller: 'FolderCtrl' })
        .otherwise({ redirectTo: '/folders/' });
}]);


// Remove "#" from URLs if the browser understands that (and the server has URL rewriting set up)
// $locationProvider.html5mode(true);


//#region Ng directives
/*  We extend Angular with custom data bindings written as Ng directives */
fsix.directive('onFocus', function () {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            elm.bind('focus', function () {
                scope.$apply(attrs.onFocus);
            });
        }
    };
})
    .directive('onBlur', function () {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                elm.bind('blur', function () {
                    scope.$apply(attrs.onBlur);
                });
            }
        };
    })
    .directive('onEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.onEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('selectedWhen', function () {
        return function (scope, elm, attrs) {
            scope.$watch(attrs.selectedWhen, function (shouldBeSelected) {
                if (shouldBeSelected) {
                    elm.select();
                }
            });
        };
    });
//if (!Modernizr.input.placeholder) {
//    // this browser does not support HTML5 placeholders
//    // see http://stackoverflow.com/questions/14777841/angularjs-inputplaceholder-directive-breaking-with-ng-model
//    todo.directive('placeholder', function () {
//        return {
//            restrict: 'A',
//            require: 'ngModel',
//            link: function (scope, element, attr, ctrl) {

//                var value;

//                var placeholder = function () {
//                    element.val(attr.placeholder);
//                };
//                var unplaceholder = function () {
//                    element.val('');
//                };

//                scope.$watch(attr.ngModel, function (val) {
//                    value = val || '';
//                });

//                element.bind('focus', function () {
//                    if (value == '') unplaceholder();
//                });

//                element.bind('blur', function () {
//                    if (element.val() == '') placeholder();
//                });

//                ctrl.$formatters.unshift(function (val) {
//                    if (!val) {
//                        placeholder();
//                        value = '';
//                        return attr.placeholder;
//                    }
//                    return val;
//                });
//            }
//        };
//    });
//}
//#endregion 