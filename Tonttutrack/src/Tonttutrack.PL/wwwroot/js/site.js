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

    setInterval(function () {
        $.ajax({
            url: '/trackerDevice/readRoutePoint',
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                console.log(data);
                marker = L.marker([data.latitude, data.longitude]);
                marker.bindPopup(`${data.latitude} <br> ${data.longitude} <br> ${data.currentSpeed}`).openPopup();
                map.addLayer(marker);
                map.setView(new L.LatLng(data.latitude, data.longitude), 10);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseJSON.message);
            }
        });

        if (marker)
        {
            map.removeLayer(marker);
        }
    }, 5000);
}

window.addEventListener("load", (event) => {
    if (sessionStorage.getItem('shouldInitReadRoutePoints') === 'true')
    {
        readRoutePoints();
        toggleDeviceViewMode();
    }
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