fsix.factory('model', ['moment', function (moment) {

    var dataservice;

    extendFolder();

    var model = {
        initialize: initialize
    };

    return model;

    //#region Internal methods

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Folder", Folder, folderInitializer);
        store.registerEntityTypeCtor("Item", null, itemInitializer);
        store.registerEntityTypeCtor("Permission", null, permissionInitializer);
    }

    function folderInitializer(folder) {
        folder.errorMessage = "";
        folder.url = "/folder/" + folder.id;
        //var expirationMoment = new moment(folder.expirationDate);
        //folder.relativeExpirationDate = expirationMoment.fromNow();
        folder.relativeExpirationDate = new moment(folder.expirationDate).fromNow();

        // Temporary properties for a new item in this folder, before it's saved to the database
        //folder.newItemName = "";
        //folder.newItemNote = "";
        //folder.newItemMIMEType = "";
        //folder.newItemContent = null;
        //folder.newItemType = "";
        folder.resetNewItemProperties();
    }

    function itemInitializer(item) {
        item.errorMessage = "";
        item.relativeCreatedDate = new moment(item.createdTime).fromNow();
        item.url = "/media.aspx?id=" + item.id;
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

    function Folder() {}

    function extendFolder() {

        Folder.prototype.addItem = function () {
            var folder = this;
            //var type = folder.newItemType;
            var type = "Note";
            var note = folder.newItemNote;
            var fileName = folder.newItemFileName;
            var mimeType = folder.newItemMimeType;
            var content = folder.newItemContent;

            // Is Breeze throwing a validation error becuase these are missing?
            var createdByUsername = "JeffGarretson";        // Server won't trust client-supplied username anyway
            var createdTime = new Date(Date.now());      // Server won't trust this...
            var modifiedTime = createdTime;    // ...or this, either

            if (note || (fileName && mimeType && content)) {
                var item = dataservice.createItem();
                item.type = type;
                item.note = note;
                item.fileName = fileName;
                item.mimeType = mimeType;
                item.content = content;
                item.folder = folder;
                
                item.createdByUsername = createdByUsername;
                item.createdTime = createdTime;
                item.modifiedTime = modifiedTime;
                item.relativeCreatedDate = new moment(item.createdTime).fromNow();

                dataservice.saveEntity(item);
                folder.resetNewItemProperties();
            }
        };

        Folder.prototype.resetNewItemProperties = function () {
            this.newItemType = "";
            this.newItemNote = "";
            this.newItemFileName = "";
            this.newItemMimeType = "";
            this.newItemContent = null;
        };

        Folder.prototype.deleteItem = function (item) {
            return dataservice.deleteItem(item);
        };

    }

    //#endregion
}]);
