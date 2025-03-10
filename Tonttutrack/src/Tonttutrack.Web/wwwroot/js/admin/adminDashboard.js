let devices = [];

$(function () {
    fetchAndRenderDevices();
});

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

// Обновяване на интерфейса
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
                    <button class="device-btn device-save-btn" data-code="${device.code}">Update</button>
                </div>
            </div>
        </div>
    `;
}

// Показване на форма за добавяне на ново устройство
$("#add-device-btn").on('click', function () {
    $(".device-add-container").toggleClass("device-hidden");
});

// изпращане на форма за добавяне на ново устройство
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

$(document).on("click", ".device-remove-btn", function () {
    if (confirm("Are you sure you want to delete this device?")) {
        const deviceId = $(this).data("id");
        devices = devices.filter(d => d.id !== deviceId);
        deleteDeviceRequest(deviceId).done(function () {
            devices = [];
            fetchAndRenderDevices();
        });
    }
});









// Показване на форма за редактиране на устройство
/*function showUpdateForm(code) {
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

// Добавяне на Event Listener за бутона "Update Device"
$(document).on("click", ".device-edit-btn", function () {
    let code = $(this).data("code");
    showUpdateForm(code);
});







// Инициализация на Event Listeners
$(document).ready(function () {
    renderDevices();

    // Event Listener за бутона за редактиране
    $(document).on("click", ".device-edit-btn", function () {
        let code = $(this).data("code");
        showUpdateForm(code);
    });
});*/