// Folder Model
define(['moment'], function (moment) {

    var dataservice;

    var model = {
        initialize: initialize
    };

    extendFolder();

    return model;

    //#region Internal methods

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Folder", Folder, folderInitializer);
    }

    function Folder() {
    }

    function folderInitializer(folder) {
        folder.errorMessage = ko.observable("");
        folder.url = ko.observable("/#folder/" + folder.id());
        folder.relativeExpirationDate = ko.observable(new moment(folder.expirationDate()).fromNow());

        folder.newItemType = ko.observable("");
        folder.newItemNote = ko.observable("");
        folder.newItemFileName = ko.observable("");
        folder.newItemMimeType = ko.observable("");
        folder.newItemContent = ko.observable(null);

        folder.editing = ko.observable(false);
    }

    function extendFolder() {

        Folder.prototype.addItem = function () {
            var folder = this;
            //var type = folder.newItemType;
            //var type = "Note";
            //var note = folder.newItemNote;
            //var fileName = folder.newItemFileName;
            //var mimeType = folder.newItemMimeType;
            //var content = folder.newItemContent;

            // Is Breeze throwing a validation error becuase these are missing?
            var createdByUsername = "Snoopy";    // Server won't trust client-supplied username anyway
            var createdTime = new Date(Date.now());     // Server won't trust this...
            var modifiedTime = createdTime;             // ...or this, either

            if (folder.newItemNote() || (
                    folder.newItemFileName() &&
                    folder.newItemMimeType() &&
                    folder.newItemContent())
                ) {
                var item = dataservice.createItem();
                item.type("Note");
                item.note(folder.newItemNote());
                item.fileName(folder.newItemFileName());
                item.mimeType(folder.newItemMimeType());
                item.content(folder.newItemContent());
                item.folder(folder);
                
                item.createdByUsername(createdByUsername);
                item.createdTime(createdTime);
                item.modifiedTime(modifiedTime);
                item.relativeCreatedDate(new moment(item.createdTime()).fromNow());

                dataservice.saveEntity(item);
                folder.resetNewItemProperties();
            }
        };

        Folder.prototype.resetNewItemProperties = function () {
            this.newItemType("");
            this.newItemNote("");
            this.newItemFileName("");
            this.newItemMimeType("");
            this.newItemContent(null);
        };

        Folder.prototype.deleteItem = function (item) {
            return dataservice.deleteItem(item);
        };

    }

    //#endregion
});
