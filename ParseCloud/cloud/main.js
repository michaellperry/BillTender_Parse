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

            family.get("Readers").fetch().then(function (readers) {
                var writersQuery = writers.relation("users").query();
                var readersQuery = readers.relation("users").query();
                var usersQuery = Parse.Query.or(
                    writersQuery, readersQuery);

                var pushQuery = new Parse.Query(Parse.Installation);
                pushQuery.matchesQuery('user', usersQuery);

                Parse.Push.send({
                    where: pushQuery,
                    data: {
                        alert: "Bill added for " +
                            request.object.get("Payee")
                    }
                });
            });


        });
    });
});
