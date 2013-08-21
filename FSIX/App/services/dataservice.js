fsix.factory('dataservice',
    ['breeze', 'Q', 'model', 'logger', '$timeout',
    function (breeze, Q, model, logger, $timeout) {

        logger.log("Creating dataservice", null, "dataservice", false);

        configureBreeze();
        var manager = new breeze.EntityManager("breeze/fsix");
        //manager.enableSaveQueuing(true);

        var dataservice = {
            metadataStore: manager.metadataStore,
            saveEntity: saveEntity,
            getFolders: getFolders,
            getFolderDetails: getFolderDetails,
            createFolder: createFolder,
            createItem: createItem,
            deleteFolder: deleteFolder,
            deleteItem: deleteItem
        };
        model.initialize(dataservice);
        return dataservice;

        /*** implementation details ***/

        //#region main application operations
        function getFolders(forceRefresh) {
            var query = breeze.EntityQuery
                    .from("folders")
                    .orderBy("expirationDate DESC, name");
            return manager.executeQuery(query)
                .then(getSucceeded);
        }

        function getFolderDetails(id) {
            var query = breeze.EntityQuery
                    .from("folders")
                    .where("id", "==", id)
                    .orderBy("expirationDate DESC, name")
                    .expand("permissions, permissions.user, items, items.createdby, logs");
            return manager.executeQuery(query)
                .then(getSucceeded);
        }

        function getSucceeded(data) {
            var qType = data.XHR ? "remote" : "local";
            logger.log(qType + " query succeeded");
            return data.results;
        }

        function createFolder() {
            return manager.createEntity("Folder");
        }

        function createItem() {
            return manager.createEntity("Item");
        }

        function deleteItem(item) {
            item.entityAspect.setDeleted();
            return saveEntity(item);
        }

        function deleteFolder(folder) {
            // Neither breeze nor server cascade deletes so we have to do it
            var items = folder.items.slice();
            items.forEach(function (entity) { entity.entityAspect.setDeleted(); });
            folder.entityAspect.setDeleted();
            return saveEntity(folder);
        }

        function saveEntity(masterEntity) {
            // if nothing to save, return a resolved promise
            if (!manager.hasChanges()) { return Q(); }

            var description = describeSaveOperation(masterEntity);
            return manager.saveChanges().then(saveSucceeded).fail(saveFailed);

            function saveSucceeded() {
                logger.log("saved " + description);
            }

            function saveFailed(error) {
                var msg = "Error saving " +
                    description + ": " +
                    getErrorMessage(error);

                masterEntity.errorMessage = msg;
                logger.log(msg, 'error');
                // Let user see invalid value briefly before reverting
                $timeout(function () { manager.rejectChanges(); }, 1000);
                throw error; // so caller can see failure
            }
        }
        function describeSaveOperation(entity) {
            var statename = entity.entityAspect.entityState.name.toLowerCase();
            var typeName = entity.entityType.shortName;
            var title = entity.title;
            title = title ? (" '" + title + "'") : "";
            return statename + " " + typeName + title;
        }
        function getErrorMessage(error) {
            var reason = error.message;
            if (reason.match(/validation error/i)) {
                reason = getValidationErrorMessage(error);
            }
            return reason;
        }
        function getValidationErrorMessage(error) {
            try { // return the first error message
                var firstItem = error.entitiesWithErrors[0];
                var firstError = firstItem.entityAspect.getValidationErrors()[0];
                return firstError.errorMessage;
            } catch (e) { // ignore problem extracting error message 
                return "validation error";
            }
        }

        //function saveChanges() {
        //    return manager.saveChanges()
        //        .then(saveSucceeded)
        //        .fail(saveFailed);

        //    function saveSucceeded(saveResult) {
        //        logger.success("# of items saved = " + saveResult.entities.length);
        //        logger.log(saveResult);
        //    }

        //    function saveFailed(error) {
        //        var reason = error.message;
        //        var detail = error.detail;

        //        if (error.entityErrors) {
        //            reason = handleSaveValidationError(error);
        //        } else if (detail && detail.ExceptionType &&
        //            detail.ExceptionType.indexOf('OptimisticConcurrencyException') !== -1) {
        //            // Concurrency error 
        //            reason =
        //                "Another user, perhaps the server, " +
        //                "may have deleted one or all of the items." +
        //                " You may have to restart the app.";
        //        } else {
        //            reason = "Failed to save changes: " + reason +
        //                     " You may have to restart the app.";
        //        }

        //        logger.error(error, reason);
        //        // DEMO ONLY: discard all pending changes
        //        // Let them see the error for a second before rejecting changes
        //        setTimeout(function () {
        //            manager.rejectChanges();
        //        }, 1000);
        //        throw error; // so caller can see it
        //    }
        //}

        //function handleSaveValidationError(error) {
        //    var message = "Not saved due to validation error";
        //    try { // fish out the first error
        //        var firstErr = error.entityErrors[0];
        //        message += ": " + firstErr.errorMessage;
        //    } catch (e) { /* eat it for now */ }
        //    return message;
        //}

        function configureBreeze() {
            // configure to use the model library for Angular
            breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);

            // configure to use camelCase
            breeze.NamingConvention.camelCase.setAsDefault();

            // configure to resist CSRF attack
            //var antiForgeryToken = $("#antiForgeryToken").val();
            //if (antiForgeryToken) {
            //    // get the current default Breeze AJAX adapter & add header
            //    var ajaxAdapter = breeze.config.getAdapterInstance("ajax");
            //    ajaxAdapter.defaultSettings = {
            //        headers: {
            //            'RequestVerificationToken': antiForgeryToken
            //        },
            //    };
            //}
        }

        //#endregion

    }]);
