define(function () {

    var model = {
        initialize: initialize
    };
    return model;

    //#region Internal methods

    var dataservice;

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Permission", null, permissionInitializer);
    }

    function permissionInitializer(permission) {
        permission.errorMessage = ko.observable("");
        permission.accessDescription = ko.observable();
        if (permission.isOwner()) {
            permission.accessDescription("Owner");
        } else {
            var perms = [];
            if (permission.permRead()) { perms.push("Read"); }
            if (permission.permWrite()) { perms.push("Write"); }
            if (permission.permShare()) { perms.push("Share"); }
            permission.accessDescription(perms.join(", "));
        }
    }

    //#endregion

});
