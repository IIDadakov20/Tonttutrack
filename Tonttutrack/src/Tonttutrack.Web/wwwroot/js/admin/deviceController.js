// извличане на устройства
function fetchDevices() {
    return $.ajax({
        url: '/device/getDevices',
        type: 'GET',
        contentType: 'application/json',
    }).fail(function (xhr) {
        let errorMessage = xhr.responseJSON.message;
        alert(errorMessage);
    });
}

// добавяне на устройство
function addDeviceRequest(name, code, password) {
    let formData = {
        Name: name,
        PasswordHash: password,
        Code: code
    };

    return $.ajax({
        type: 'POST',
        url: '/device/createOrUpdateDevice',
        contentType: 'application/json',
        data: JSON.stringify(formData)
    }).fail(function (xhr) {
        let errorMessage = xhr.responseJSON.message;
        alert(errorMessage);
    });
}

// Обновяване на устройство
function updateDeviceRequest(deviceCode, deviceName, devicePassword) {
    let formData = {
        Name: deviceName,
        PasswordHash: devicePassword,
        Code: deviceCode
    };

    return $.ajax({
        type: 'POST',
        url: '/device/createOrUpdateDevice',
        contentType: 'application/json',
        data: JSON.stringify(formData)
    }).fail(function (xhr) {
        let errorMessage = xhr.responseJSON.message;
        alert(errorMessage);
    });
}

// изтриване на устройство
function deleteDeviceRequest(id) {
    return $.ajax({
        type: 'DELETE',
        url: '/device/deleteDevice',
        contentType: 'application/json',
        data: JSON.stringify(id)
    }).fail(function (xhr) {
        let errorMessage = xhr.responseJSON.message;
        alert(errorMessage);
    });
}