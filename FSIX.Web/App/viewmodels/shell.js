define(['plugins/router', 'durandal/app'], function (router, app) {
    "use strict";

    return {
        router: router,
        search: function() {
            app.showMessage('Search not yet implemented...');
        },
        activate: function () {
            router
                .makeRelative({
                    moduleId: 'viewmodels'
                }).map([
                    { route: '', title: 'Home', moduleId: 'home', nav: true },
                    { route: 'folders', title: 'Folders', moduleId: 'folders', nav: true },
                    { route: 'expired', title: 'Expired Folders', moduleId: 'expired' },
                    { route: 'folder/:id', title: 'Folder Details', moduleId: 'folder' },
                    { route: 'new', title: 'New Folder', moduleId: 'new' },
                    { route: 'help', title: 'Help', moduleId: 'help', nav: true },
                    { route: 'admin', title: 'Admin', moduleId: 'admin', nav: true }
                ]).buildNavigationModel();
            
            return router.activate({
                //pushState: true
            });
        }
    };
});