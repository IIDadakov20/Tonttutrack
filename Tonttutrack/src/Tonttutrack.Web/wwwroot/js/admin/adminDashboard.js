let devices = [
    { code: "00:1A:2B:3C:4D:5E", name: "Device 1", password: "password1" }
];

renderDevices();

// Добавяне на ново устройство
function addDevice(name, code, password) {
    let existingDevice = devices.find(d => d.code === code);
    if (existingDevice) {
        alert("Device with this code already exists!");
        return;
    }

    let device = { name, code, password };
    devices.push(device);
    renderDevices();
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

// Изтриване на устройство
function deleteDevice(code) {
    devices = devices.filter(d => d.code !== code);

    renderDevices();
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
                <button class="device-btn device-remove-btn" data-code="${device.code}">Delete Device</button>
                <div class="device-update-container device-hidden">
                    <input type="password" class="device-password-input" placeholder="New Password">
                    <button class="device-btn device-save-btn" data-code="${device.code}">Update</button>
                </div>
            </div>
        </div>
    `;
}

// Показване на форма за редактиране
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

// Добавяне на Event Listener за бутона "Update Device"
$(document).on("click", ".device-edit-btn", function () {
    let code = $(this).data("code");
    showUpdateForm(code);
});

// Потвърждение за изтриване на устройство
function confirmDeleteDevice(code) {
    if (confirm("Are you sure you want to delete this device?")) {
        deleteDevice(code);
    }
}

// Показване на форма за добавяне на ново устройство
function toggleAddDeviceForm() {
    $(".device-add-container").toggleClass("device-hidden");
}

// Добавяне на ново устройство
function addNewDevice() {
    let newName = $("#new-device-name").val().trim();
    let newCode = $("#new-device-code").val().trim();
    let newPassword = $("#new-device-password").val().trim();
    
    if (newName && newCode && newPassword) {
        addDevice(newName, newCode, newPassword);
        $(".device-add-container").addClass("device-hidden");
        $("#new-device-name").val("");
        $("#new-device-code").val("");
        $("#new-device-password").val("");
    } else {
        /////////////////////////////////////////////////
    }
}

// Инициализация на Event Listeners
$(document).ready(function () {
    renderDevices();

    // Event Listener за бутона за редактиране
    $(document).on("click", ".device-edit-btn", function () {
        let code = $(this).data("code");
        showUpdateForm(code);
    });

    // Event Listener за бутона за изтриване
    $(document).on("click", ".device-remove-btn", function () {
        let code = $(this).data("code");
        confirmDeleteDevice(code);
    });

    // Отваряне на формата за добавяне на у-во
    $("#add-device-btn").click(function() {
        toggleAddDeviceForm();
    });

    // Event Listener за бутона за добавяне на ново устройство
    $("#save-new-device-btn").click(function() {
        addNewDevice();
    });
});