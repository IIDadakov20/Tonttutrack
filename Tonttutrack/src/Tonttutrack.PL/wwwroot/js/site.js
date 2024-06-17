$(function () {
    $('#deviceConnectionForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            Code: $('#Code').val(),
            Password: $('#Password').val()
        };

        // Send AJAX request
        $.ajax({
            url: '/trackerDevice/connectDevice',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response === true) {
                    readRoutePoints();
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseJSON.message);
            }
        });
    });
});

function readRoutePoints() {
    setInterval(function () {
        $.ajax({
            url: '/trackerDevice/readRoutePoint',
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseJSON.message);
            }
        });
    }, 5000);
}