let devices = [];

$(function () {
    fetchAndRenderDevices();
});


// извличане на устройствата и онбновява на листа
function fetchAndRenderDevices() {
    fetchDevices().done(function (fetchedDevices) {
        fetchedDevices.forEach(device => {
            if (!devices.some(d => d.id === device.id)) {
                devices.push(device);
            }
        });
        renderDevices();
    });
}

// Обновяване на листа с устройствата
function renderDevices() {
    $(".device-container").empty();
    devices.forEach(device => {
        let newRow = createDeviceRow(device);
        $(".device-container").append(newRow);
    });
}

// Създаване на HTML ред за устройство
function createDeviceRow(device) {
    let escapedCode = device.code.replace(/:/g, "-");
    return `
        <div class="device-row" id="row-${escapedCode}">
            <div class="device-column">
                <span class="device-name">${device.name}</span>
                <input type="text" class="device-edit-name device-hidden" value="${device.name}">
            </div>
            <div class="device-column">${device.code}</div>
            <div class="device-column">
                <button class="device-btn device-edit-btn" data-code="${device.code}">Update Device</button>
                <button class="device-btn device-remove-btn" data-id="${device.id}">Delete Device</button>
                <div class="device-update-container device-hidden">
                    <input type="password" class="device-password-input" placeholder="New Password">
                    <button class="device-btn device-save-btn device-update-save-btn" data-code="${device.code}">Update</button>
                </div>
            </div>
        </div>
    `;
}

// Показване на форма за добавяне на ново устройство
$("#add-device-btn").on('click', function () {
    $(".device-add-container").toggleClass("device-hidden");
});

// верифициране на форма за добавяне на ново устройство при натискане на бутона
$("#save-new-device-btn").on('click', function () {
    let newName = $("#new-device-name").val().trim();
    let newCode = $("#new-device-code").val().trim();
    let newPassword = $("#new-device-password").val().trim();

    if (newName && newCode && newPassword) {
        addDevice(newName, newCode, newPassword);
        $(".device-add-container").addClass("device-hidden");
        $("#new-device-name").val("");
        $("#new-device-code").val("");
        $("#new-device-password").val("");
    }
    else {
        alert("All fields are requred");
    }
});

// изпращане на заявка за добавяне на ново устройство
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

// изпращане на заявка за изтриване на устройство при натискане на бутона
$(document).on("click", ".device-remove-btn", function () {
    if (confirm("Are you sure you want to delete this device?")) {
        const deviceId = $(this).data("id");
        deleteDeviceRequest(deviceId).done(function () {
            devices = [];
            fetchAndRenderDevices();
        });
    }
});

// Показване на форма за редактиране на устройство
$(document).on("click", ".device-edit-btn", function () {
    if (confirm("Are you sure you want to update this device?")) {
        const deviceCode = $(this).data("code");
        showUpdateForm(deviceCode);
    }
});

function showUpdateForm(code) {
    let escapedCode = code.replace(/:/g, "-");
    let row = $("#row-" + escapedCode);
    if (row.length === 0) {
        return;
    }

    row.find(".device-name").addClass("device-hidden");
    row.find(".device-edit-name").removeClass("device-hidden");
    row.find(".device-update-container").removeClass("device-hidden");
    row.find(".device-edit-btn, .device-remove-btn").addClass("device-hidden");
}

// изпращане на заявка за обновяване на устройство при натискане на бутона
$(document).on("click", ".device-update-save-btn", function () {
    const deviceCode = $(this).data("code");
    let device = devices.find(d => String(d.code).trim() === deviceCode);
    updateDeviceRequest(deviceCode, device.name, device.password).done(function () {
        devices = [];
        fetchAndRenderDevices();
    });
});