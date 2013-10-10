// Media Model
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
        store.registerEntityTypeCtor("Media", null, mediaInitializer);
    }

    function mediaInitializer(media) {
        media.url = ko.computed(function () {
            return "/api/media/" + media.id();
        });
    }

});
