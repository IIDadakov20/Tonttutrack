$('#record-btn').on('click', function () {
    $('#deviceInfoView .text-danger').html('');

    $(this).find('#record-btn').prop('disabled', true);

    $.ajax({
        type: 'POST',
        url: '/route/createRoute',
        contentType: 'application/json',
        success: function (response) {
            sessionStorage.setItem('routeRecord', true);
            sessionStorage.setItem('route', response);
            toggleRouteRecord();
        },
        error: function (xhr) {
            let errorMessage = xhr.responseJSON.message;
            $('#deviceInfoView').find('.formErrorMessage')
                .html(errorMessage).show();
        },
        complete: function () {
            $('#record-btn').prop('disabled', false);
        }
    });
});

function toggleRouteRecord() {
    if (sessionStorage.getItem('routeRecord') === 'true') {
        $('#saveRecord-btn').removeClass('hidden').addClass('visible');
        $('#record-btn').addClass('hidden');
    }
    else{
        $('#record-btn').removeClass('hidden').addClass('visible');
        $('#saveRecord-btn').addClass('hidden');
    }
}

function saveRoutePoint(routePoint) {
    $.ajax({
        type: 'POST',
        url: '/route/saveRoutePoint',
        contentType: 'application/json',
        data: JSON.stringify({
            route: sessionStorage.getItem('route'),
            routePoint: routePoint
        }),
        success: function (response) {
            console.log('Success:', response);
            sessionStorage.setItem('lastRoutePoint', routePoint);
        },
        error: function (status, error) {
            console.error('Error:', status, error);
        }
    });
}