// Форма за свързване на устройството
$('#deviceConnectionForm').on('submit', function (e) {
    e.preventDefault();

    var formData = {
        Code: $('#Code').val(),
        Password: $('#passwordInput').val()
    };

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
                //toggleDeviceViewMode();
                readRoutePoints();
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseJSON.message);
        }
    });
});

// Изключване на устройството
$('#disconnectButton').on('click', function (e) {
    e.preventDefault();

    $.ajax({
        url: '/trackerDevice/disconnectDevice',
        type: 'DELETE',
        contentType: 'application/json',
    });
});

////////////////////////////////////////////////////////////
// Dropdown функционалност за акаунт
$(function () {
    const dropdownButton = document.getElementById('custom-dropdown-button');
    const dropdownMenu = document.getElementById('custom-dropdown-menu');

    dropdownButton.addEventListener('click', function (e) {
        e.stopPropagation();
        dropdownMenu.classList.toggle('show');
    });

    document.addEventListener('click', function (e) {
        if (!dropdownMenu.contains(e.target) && !dropdownButton.contains(e.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});

////////////////////////////////////////////////////////////
// Четене на точките от маршрута
function readRoutePoints() {
    var marker;

    var intervalId = setInterval(function () {
        $.ajax({
            url: '/trackerDevice/readRoutePoint',
            type: 'GET',
            contentType: 'application/json',
            data: { deviceCode: sessionStorage.getItem("deviceCode") },
            success: function (data) {
                if (data === 'break') {
                    sessionStorage.removeItem('connectedDeviceName');
                    sessionStorage.removeItem('shouldInitReadRoutePoints');
                    clearInterval(intervalId);
                    if (marker) {
                        map.removeLayer(marker);
                    }
                    //toggleDeviceViewMode();
                    return;
                }

                if (sessionStorage.getItem("routeRecord")) {
                    saveRoutePoint(data);
                }

                if (window.location.pathname === "/Map/MapTrackerLayout") {
                    console.log(data);

                    if (marker) {
                        map.removeLayer(marker);
                    }

                    marker = L.marker([data.latitude, data.longitude]);
                    marker.bindPopup(`${data.latitude} <br> ${data.longitude} <br> ${data.currentSpeed}`).openPopup();
                    map.addLayer(marker);
                    map.setView(new L.LatLng(data.latitude, data.longitude), 16);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseJSON?.message || 'Error reading route points.');
            }
        });
    }, 2000);
}

window.addEventListener("load", () => {
    if (sessionStorage.getItem('shouldInitReadRoutePoints') === 'true') {
        readRoutePoints();
        //toggleDeviceViewMode();
    }
});

$('#userUpdateForm').on('submit', function (e) {
    e.preventDefault();

    var formData = {
        Username: $('#newUserUsername').val(),
        Email: $('#newUserEmail').val()
    };

    $('#userUpdateForm .text-danger').html('');

    $(this).find('.submitButton').prop('disabled', true); // Disable submit button

    $.ajax({
        url: '/user/updateUser',
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            $('#account-name').text(formData.Username);
        },
        error: function (xhr) {
            var errors = xhr.responseJSON;

            // Loop through the errors and show them under the corresponding input field
            for (var field in errors) {
                var fieldErrors = errors[field];  // Array of error messages for the field
                if (fieldErrors && fieldErrors.length > 0) {
                    // Display the error message for the respective field
                    var errorMessage = fieldErrors.join('<br />');  // Join multiple errors into one string
                    if (field == "") {
                        $('#userUpdateForm').find('.formErrorMessage').html(errorMessage);
                        document.getElementsByClassName("formErrorMessage")[0].style.display = "block";
                    }
                    else {
                        // Find the span next to the input field and insert the error message
                        $('input[name="Item1.' + field + '"]').next('.text-danger').html(errorMessage);
                    }
                }
            }
        },
        complete: function () {
            $('.submitButton').prop('disabled', false); // Re-enable submit button
        }
    });
});

$('#userPasswordUpdateForm').on('submit', function (e) {
    e.preventDefault();

    var formData = {
        CurrentPassword: $('#currentPassword').val(),
        NewPassword: $('#newPassword').val(),
        ConfirmPassword: $('#confirmPassword').val()
    };

    $('#userPasswordUpdateForm .text-danger').html('');

    $(this).find('.submitButton').prop('disabled', true); // Disable submit button

    $.ajax({
        url: '/user/updateUserPassword',
        type: 'PATCH',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) { },
        error: function (xhr) {
            var errors = xhr.responseJSON;

            // Loop through the errors and show them under the corresponding input field
            for (var field in errors) {
                var fieldErrors = errors[field];  // Array of error messages for the field
                if (fieldErrors && fieldErrors.length > 0) {
                    // Display the error message for the respective field
                    var errorMessage = fieldErrors.join('<br />');  // Join multiple errors into one string
                    if (field == "") {
                        $('#userPasswordUpdateForm').find('.formErrorMessage').html(errorMessage);
                        document.getElementsByClassName("formErrorMessage")[1].style.display = "block";
                    } else {
                        // Find the span next to the input field and insert the error message
                        $('input[name="Item2.' + field + '"]').next('.text-danger').html(errorMessage);
                    }
                }
            }
        },
        complete: function () {
            $('.submitButton').prop('disabled', false); // Re-enable submit button
        }
    });
});

document.getElementById('record-btn').addEventListener('click', () => {
    $.ajax({
        type: 'POST',
        url: '/trackerDevice/createRoute',
        contentType: 'application/json',
        success: function (response) {
            sessionStorage.setItem('routeRecord', true);
            sessionStorage.setItem('route', response);
        },
        error: function (status, error) {
            console.error('Error:', status, error);
        }
    });
});

function saveRoutePoint(routePoint) {
    $.ajax({
        type: 'POST',
        url: '/trackerDevice/saveRoutePoint',
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