// Форма за свързване на устройството
$('#deviceConnectionForm').on('submit', function (e) {
    e.preventDefault();

    let formData = {
        Code: $('#Code').val(),
        Password: $('#passwordInput').val()
    };

    $('#deviceConnectionForm .text-danger').html('');

    $(this).find('#connect-btn').prop('disabled', true);

    $.ajax({
        url: '/trackerDevice/connectDevice',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success === true) {
                sessionStorage.setItem('deviceCode', formData.Code)
                sessionStorage.setItem('shouldInitReadRoutePoints', 'true');
                sessionStorage.setItem('connectedDeviceName', response.deviceName);
                readRoutePoints();
                showDisconnect();
            }
        },
        error: function (xhr) {
            let error = xhr.responseJSON;
            let errorMessage = error.message;
            $(this).find('.formErrorMessage').html(errorMessage).show();
        },
        complete: function () {
            $('#connect-btn').prop('disabled', false);
        }
    });
});

function showDisconnect() {
    $('#deviceConnectionForm').addClass('hidden');
    $('#deviceInfoView').removeClass('hidden').addClass('visible');

    $('#connectedDeviceName').text(sessionStorage.getItem('connectedDeviceName'));
}

// Изключване на устройството
$('#disconnectButton').on('click', function (e) {
    e.preventDefault();

    let deviceCode = sessionStorage.getItem("deviceCode");

    $('#deviceConnectionForm .text-danger').html('');

    $(this).find('#disconnectButton').prop('disabled', true);

    $.ajax({
        url: '/trackerDevice/disconnectDevice',
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify(deviceCode),
        success: function () {
            sessionStorage.removeItem('deviceCode');
            sessionStorage.removeItem('shouldInitReadRoutePoints');
            sessionStorage.removeItem('connectedDeviceName');
            showConnect();
        },
        error: function (xhr) {
            let error = xhr.responseJSON;
            let errorMessage = error.message;
            $(this).find('.formErrorMessage').html(errorMessage).show();
        },
        complete: function () {
            $('#disconnectButton').prop('disabled', false);
        }
    });
});

function showConnect() {
    $('#deviceConnectionForm').removeClass('hidden').addClass('visible');
    $('#deviceInfoView').addClass('hidden');

    $('#connectedDeviceName').text('');
}

// Четене на точките от маршрута
function readRoutePoints() {
    let marker;

    let intervalId = setInterval(function () {
        $.ajax({
            url: '/trackerDevice/readRoutePoint',
            type: 'GET',
            contentType: 'application/json',
            data: { deviceCode: sessionStorage.getItem("deviceCode") },
            success: function (data) {
                if (sessionStorage.getItem('shouldInitReadRoutePoints') != 'true') {
                    sessionStorage.removeItem('connectedDeviceName');
                    sessionStorage.removeItem('deviceCode')
                    clearInterval(intervalId);
                    if (marker) {
                        map.removeLayer(marker);
                    }
                    showConnect();
                    return;
                }

                if (sessionStorage.getItem("routeRecord")) {
                    saveRoutePoint(data);
                }

                if (window.location.pathname === "/Map/MapTrackerLayout") {
                    if (marker) {
                        map.removeLayer(marker);
                    }

                    marker = L.marker([data.latitude, data.longitude]);
                    marker.bindPopup(`${data.latitude} <br> ${data.longitude} <br> ${data.currentSpeed}`).openPopup();
                    map.addLayer(marker);
                    map.setView(new L.LatLng(data.latitude, data.longitude), 16);
                }
            },
            error: function (xhr) {
                console.error(xhr.responseJSON?.message || 'Error reading route points.');
            }
        });
    }, 2000);
}

$(window).on('load', function () {
    if (sessionStorage.getItem('shouldInitReadRoutePoints') === 'true') {
        readRoutePoints();
        showDisconnect();
    }
});