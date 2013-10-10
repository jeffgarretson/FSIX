// Item Model
define(['moment'], function (moment) {
    "use strict";

    var dataservice;

    var model = {
        initialize: initialize
    };

    return model;

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Item", null, itemInitializer);
    }

    function itemInitializer(item) {
        item.errorMessage = ko.observable("");
        item.relativeCreatedDate = ko.computed(function () {
            return new moment(item.createdTime()).fromNow();
        });
        item.fileUploadUrl = ko.computed(function () {
            return "/api/media/" + item.id();
        });
    }

});
