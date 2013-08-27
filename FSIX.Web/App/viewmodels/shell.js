define(['plugins/router', 'durandal/app'], function (router, app) {
    return {
        router: router,
        search: function() {
            //It's really easy to show a message box.
            //You can add custom options too. Also, it returns a promise for the user's response.
            app.showMessage('Search not yet implemented...');
        },
        activate: function () {
            router.map([
                { route: '', title: 'Folders', moduleId: 'viewmodels/folders', nav: true },
                { route: 'expired', title: 'Expired Folders', moduleId: 'viewmodels/expired' },
                { route: 'folder/:id', title: 'Folder Details', moduleId: 'viewmodels/folder' },
                { route: 'new', title: 'New Folder', moduleId: 'viewmodels/new', nav: true },
                { route: 'help', title: 'Help', moduleId: 'viewmodels/help', nav: true },
                { route: 'admin', title: 'Admin', moduleId: 'viewmodels/admin', nav: true },

                { route: 'welcome', title: 'Welcome', moduleId: 'viewmodels/welcome' },
                { route: 'flickr', moduleId: 'viewmodels/flickr' }
            ]).buildNavigationModel();
            
            return router.activate({
                //pushState: true
            });
        }
    };
});