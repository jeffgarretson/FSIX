fsix.factory('model', ['moment', function (moment) {

    var dataservice;

    var model = {
        initialize: initialize
    };

    return model;

    //#region Internal methods
    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Folder", null, folderInitializer);
        store.registerEntityTypeCtor("Item", null, itemInitializer);
        store.registerEntityTypeCtor("Permission", null, permissionInitializer);
    }

    function folderInitializer(folder) {
        folder.errorMessage = "";
        folder.url = "/folder/" + folder.id;
        var expirationMoment = new moment(folder.expirationDate);
        folder.relativeExpirationDate = expirationMoment.fromNow();
    }

    function itemInitializer(item) {
        item.errorMessage = "";
    }

    function permissionInitializer(permission) {
        permission.errorMessage = "";
        if (permission.isOwner) {
            permission.accessDescription = "Owner";
        } else {
            var perms = [];
            if (permission.permRead) { perms.push("Read"); }
            if (permission.permWrite) { perms.push("Write"); }
            if (permission.permShare) { perms.push("Share"); }
            permission.accessDescription = perms.join(", ");
        }
    }
}]);
