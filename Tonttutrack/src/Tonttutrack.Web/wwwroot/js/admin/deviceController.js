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

// Добавяне на ново устройство
function addDevice(name, code, password) {
    let existingDevice = devices.find(d => d.code === code);
    if (existingDevice) {
        alert("Device with this code already exists!");
        return;
    }

    addDeviceRequest(name, code, password).done(function () {
        devices = [];
        fetchAndRenderDevices();
    });
}

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


// Изтриване на устройство
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



// Обновяване на устройство
function updateDevice(code, newName, newPassword) {
    code = String(code).trim();
    let device = devices.find(d => String(d.code).trim() === code);
    if (device) {
        device.name = newName;
        device.password = newPassword;
        renderDevices();
    } else {
    }
}