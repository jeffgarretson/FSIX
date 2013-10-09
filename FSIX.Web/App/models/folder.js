// Folder Model
define(['moment'], function (moment) {
    "use strict";

    var dataservice;

    var model = {
        initialize: initialize
    };

    extendFolder();

    return model;

    function initialize(context) {
        dataservice = context;
        var store = dataservice.metadataStore;
        store.registerEntityTypeCtor("Folder", Folder, folderInitializer);
    }

    function Folder() {
    }

    function folderInitializer(folder) {
        folder.errorMessage = ko.observable("");
        folder.url = ko.computed(function () {
            return "/#folder/" + folder.id()
        });
        folder.relativeExpirationDate = ko.computed(function () {
            return new moment(folder.expirationDate()).fromNow()
        });
    }

    function extendFolder() {
        Folder.prototype.expire = function () {
            return dataservice.expireFolder(this);
        };
        Folder.prototype.deleteItem = function (item) {
            return dataservice.deleteItem(item);
        };
    }

});
