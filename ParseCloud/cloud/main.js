Parse.Cloud.beforeSave("Bill", function (request, response) {
    request.object.get("Family").fetch().then(function (family) {
        family.get("Writers").fetch().then(function (writers) {
            writers.relation("users").query().find().then(function (results) {
                var found = false;
                for (var i = 0; i < results.length; ++i) {
                    if (results[i].id === request.user.id) {
                        found = true;
                        break;
                    }
                }
                if (found)
                    response.success();
                else
                    response.error("You cannot write to the family");
            });
        });
    });
});
