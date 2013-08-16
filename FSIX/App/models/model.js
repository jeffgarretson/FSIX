fsix.factory('model', function () {

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
    }

    function folderInitializer(folder) {
        folder.errorMessage = "";
        // Is this where I should set the URL property?
        folder.url = "/folder/" + folder.id;
    }

    function itemInitializer(item) {
        item.errorMessage = "";
    }

});