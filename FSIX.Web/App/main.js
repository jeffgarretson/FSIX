requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions',
        'breeze': '../Scripts/breeze.debug',
        'toastr': '../Scripts/toastr',
        'logger': 'services/logger',
        'dataservice': 'services/dataservice',
        'moment': '../Scripts/moment',
        'plupload': '../Scripts/plupload/plupload.full'
    },
    shim: {
        plupload: {
            exports: 'plupload'
        }
    }
});

define('jquery', function() { return jQuery; });
define('knockout', ko);

define(
    ['durandal/system', 'durandal/app', 'durandal/viewLocator', 'toastr'],
    function (system, app, viewLocator, toastr) {
        //>>excludeStart("build", true);
        system.debug(true);
        //>>excludeEnd("build");

        app.title = 'Frictionless Secure Information eXchange';

        app.configurePlugins({
            router: true,
            dialog: true,
            widget: true
        });

        app.start().then(function() {
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.options.backgroundpositionClass = 'toast-bottom-right';

            //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
            //Look for partial views in a 'views' folder in the root.
            viewLocator.useConvention();

            //Show the app by setting the root view model for our application with a transition.
            app.setRoot('viewmodels/shell', 'entrance');
        });
    }
);
