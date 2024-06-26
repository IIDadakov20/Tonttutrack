$(function () {
    $('#deviceConnectionForm').on('submit', function (e) {
        e.preventDefault();

        var formData = {
            Code: $('#Code').val(),
            Password: $('#Password').val()
        };

        $.ajax({
            url: '/trackerDevice/connectDevice',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.success === true) {
                    sessionStorage.setItem('shouldInitReadRoutePoints', 'true');
                    sessionStorage.setItem('connectedDeviceName', response.deviceName);
                    toggleDeviceViewMode();
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
    var marker;

    var intervalId = setInterval(function () {
        $.ajax({
            url: '/trackerDevice/readRoutePoint',
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                if (data === 'break') {
                    sessionStorage.removeItem('connectedDeviceName');
                    sessionStorage.removeItem('shouldInitReadRoutePoints');
                    clearInterval(intervalId)
                    if (marker) {
                        map.removeLayer(marker);
                    }
                    toggleDeviceViewMode();
                    return;
                }

                console.log(data);

                if (marker) {
                    map.removeLayer(marker);
                }

                marker = L.marker([data.latitude, data.longitude]);
                marker.bindPopup(`${data.latitude} <br> ${data.longitude} <br> ${data.currentSpeed}`).openPopup();
                map.addLayer(marker);
                map.setView(new L.LatLng(data.latitude, data.longitude), 10);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseJSON.message);
            }
        });
    }, 5000);
}

window.addEventListener("load", () => {
    if (sessionStorage.getItem('shouldInitReadRoutePoints') === 'true')
    {
        readRoutePoints();
        toggleDeviceViewMode();
    }
});

window.addEventListener('beforeunload', () => {
    disconnectDevice();
});

function toggleDeviceViewMode() {
    var deviceConnectionForm = document.getElementById('deviceConnectionForm');
    var deviceInfoView = document.getElementById('deviceInfoView');

    if (sessionStorage.getItem('connectedDeviceName') != null) {
        document.getElementById("connectedDeviceName").innerText = sessionStorage.getItem('connectedDeviceName');
        deviceConnectionForm.classList.remove("d-md-inline-flex");
        deviceConnectionForm.classList.add("d-none");
        deviceInfoView.classList.add("d-md-inline-flex");
        deviceInfoView.classList.remove("d-none");
    }
    else {
        deviceInfoView.classList.remove("d-md-inline-flex");
        deviceInfoView.classList.add("d-none");
        deviceConnectionForm.classList.add("d-md-inline-flex");
        deviceConnectionForm.classList.remove("d-none");
    }
}

function disconnectDevice() {
    $.ajax({
        url: '/trackerDevice/disconnectDevice',
        type: 'DELETE',
        contentType: 'application/json',
    });
}