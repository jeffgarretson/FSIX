define(['durandal/system', 'durandal/plugins/router', 'services/logger'],
    function (system, router, logger) {
        var shell = {
            activate: activate,
            router: router
        };
        
        return shell;

        //#region Internal Methods
        function activate() {
            return boot();
        }

        function boot() {
            // Routes shown on main nav bar
            router.mapNav({ url: 'active', name: 'Active' });
            router.mapNav({ url: 'expired', name: 'Expired' });
            router.mapNav({ url: 'new', name: 'New' });

            // Other routes
            router.mapRoute({ url: 'folder/:id', visible: false });

            return router.activate('active');
            log('FSIX ready.', null, true);
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });
