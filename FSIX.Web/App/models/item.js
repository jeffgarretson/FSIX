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
        //item.relativeCreatedDate = ko.observable(new moment(item.createdTime()).fromNow());
        item.relativeCreatedDate = ko.computed(function () {
            return new moment(item.createdTime()).fromNow();
        });
        //item.url = ko.observable("/media.aspx?id=" + item.id());
        item.url = ko.computed(function () {
            return "/media.aspx?id=" + item.id();
        });
    }

});
