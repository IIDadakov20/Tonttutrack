// Стартира запис на маршрут
$('#record-btn').on('click', function () {
    if (!sessionStorage.getItem('recordAccess')) {
        return;
    }

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

// Запази маршрут
$('#saveRecord-btn').on('click', function () {
    $('#deviceInfoView .text-danger').html('');

    $(this).find('#saveRecord-btn').prop('disabled', true);

    sessionStorage.removeItem('routeRecord');
    $.ajax({
        type: 'PATCH',
        url: '/route/updateRoute',
        contentType: 'application/json',
        data: JSON.stringify({
            Id: sessionStorage.getItem('route'),
            Name: "new"
        }),
        success: function () {
            sessionStorage.removeItem('route');
            toggleRouteRecord();
        },
        error: function (xhr) {
            sessionStorage.setItem('routeRecord', true);
            let errorMessage = xhr.responseJSON.message;
            $('#deviceInfoView').find('.formErrorMessage')
                .html(errorMessage).show();
        },
        complete: function () {
            $('#saveRecord-btn').prop('disabled', false);
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

// Запазване на точка от маршрут
function saveRoutePoint(routePoint) {
    $('#deviceInfoView .text-danger').html('');

    $.ajax({
        type: 'POST',
        url: '/route/saveRoutePoint',
        contentType: 'application/json',
        data: JSON.stringify({
            route: sessionStorage.getItem('route'),
            routePoint: routePoint
        }),
        success: function (response) {
            sessionStorage.setItem('lastRoutePoint', routePoint);
            $('#deviceInfoView').find('.formErrorMessage')
                .html("Route point saved.").show();
        },
        error: function (xhr) {
            let errorMessage = xhr.responseJSON.message;
            $('#deviceInfoView').find('.formErrorMessage')
                .html(errorMessage).show();
        }
    });
}

// извличане на маршрути на потребителя
function fetchUserRoutes(pageNumber) {
    return $.ajax({
        url: '/route/getRoutes',
        type: 'GET',
        contentType: 'application/json',
        data: { pageNumber: pageNumber }
    }).fail(function (xhr) {
        let errorMessage = xhr.responseJSON.message;
        $('#routes-section').find('.formErrorMessage')
            .html(errorMessage).show();
    });
}

// извичане на общия брой на маршрутите на поребителя
function fetchUserRoutesNumber() {
    $('#routes-section .text-danger').html('');

    $.ajax({
        url: '/route/getRoutesNumber',
        type: 'GET',
        contentType: 'application/json',
        success: function (response) {
            sessionStorage.setItem('totalRoutes', response);
        },
        error: function (xhr) {
            let errorMessage = xhr.responseJSON.message;
            $('#routes-section').find('.formErrorMessage')
                .html(errorMessage).show();
        }
    });
}

// обновяване на име на маршрут
$("#save-route-name ").on("click", function () {
    $('#routes-section .text-danger').html('');

    $(this).find('#save-route-name').prop('disabled', true);

    const routeNameInput = $("#route-name-input").val();
    if (routeNameInput !== "" && activeRoute !== null) {
        const currentRoute = routes.find(route => route.id === activeRoute);
        if (!currentRoute) {
            $('#routes-section').find('.formErrorMessage')
                .html("Problem occured during route update").show();
            return;
        }

        updateRouteName(currentRoute, routeNameInput).done(function (success) {
            if (success) {
                currentRoute.name = routeNameInput;
                renderRoutes();
            }
        });
    }
});

function updateRouteName(currentRoute, name) {
    return $.ajax({
        type: 'PATCH',
        url: '/route/updateRoute',
        contentType: 'application/json',
        data: JSON.stringify({
            Id: currentRoute.id,
            Name: name
        }),
        error: function (xhr) {
            sessionStorage.setItem('routeRecord', true);
            let errorMessage = xhr.responseJSON.message;
            $('#routes-section').find('.formErrorMessage')
                .html(errorMessage).show();
        },
    });
}