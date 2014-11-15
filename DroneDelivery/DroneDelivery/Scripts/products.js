(function () {

    // Backend
    function checkAvailability(id, callback)
    {
        $.get('/api/inventory/check/' + id, '', function (data, status, xhr)
        {
            if (window.requestInterval)
                window.clearInterval(window.requestInterval);

            console.log('[Inventory Check Request: ' + data.Request.Id + ']');

            var id = data.Request.Id;

            // check the inventory request every second until it has a result.. or error
            window.requestInterval = setInterval(function () {
                $.get('/api/inventory/check/' + id, '', function (data, status, xhr) {
                    console.dir(data);
                    if (data.HasResult) {
                        console.log('[Inventory Check Request: ' + data.Request.Id + '] Finished.');
                        window.clearInterval(window.requestInterval);
                        callback(data.Request);
                    } else if (data.Error) {
                        console.log("Error checking availability for product. The request has probably expired.");
                        callback(data);
                        window.clearInterval(window.requestInterval);
                    }
                });
            }, 1000);
        },'json');
    }

    window.checkAvailability = checkAvailability;

    // Products related UI
    $(function () {

        // Availability checker on the product details page.
        function DetailsPage() {
            var source = $("#availability-template").html();
            if (!source) return;

            var template = Handlebars.compile(source);
            window.availability_template = template;

            $(".btn-check-avail").click(function () {
                $(".availability-spinner").show();
                checkAvailability(window.__productId, function (data) {
                    if (data) {
                        $("#availability_info").html(template(data));
                    }

                    $("#availability_info").show();
                    $(".availability-spinner").hide();
                });
            });
        }

        function AdminPage()
        {
            if (location.href.toLocaleLowerCase().indexOf("admin") == -1) return;


        }

        DetailsPage();
        
    });

})();