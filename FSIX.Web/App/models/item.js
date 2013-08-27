define(['moment'], function (moment) {

    var dataservice;

    var model = {
        initialize: initialize
    };

    return model;

    //#region Internal methods

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Item", null, itemInitializer);
    }

    function itemInitializer(item) {
        item.errorMessage = ko.observable("");
        item.relativeCreatedDate = ko.observable(new moment(item.createdTime()).fromNow());
        item.url = ko.observable("/media.aspx?id=" + item.id());
    }

    //#endregion

});
