$(function () {
    // Форма за свързване на устройството
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
    const dropdownButton = document.querySelector('.custom-dropdown-button');
    const dropdownMenu = document.querySelector('.custom-dropdown-menu');

    dropdownButton.addEventListener('click', function (e) {
        e.stopPropagation(); // Спира пропагирането, за да не затваря веднага менюто
        dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
    });

    document.addEventListener('click', function (e) {
        // Проверка дали кликът е извън менюто
        if (!dropdownMenu.contains(e.target) && !dropdownButton.contains(e.target)) {
            dropdownMenu.style.display = 'none';
        }
    });

    ////////////////////////////////////////////////////////////
    // Функционалност за поп-ъпите
    const settingsButton = document.getElementById("settingsButton");
    const closeSettingsPopup = document.getElementById("closeSettingsPopup");
    const settingsPopup = document.getElementById("settingsPopup");

    const updateAccountButton = document.getElementById("updateAccountButton");
    const closeUpdateAccountPopup = document.getElementById("closeUpdateAccountPopup");
    const updateAccountPopup = document.getElementById("updateAccountPopup");

    const deleteAccountButton = document.getElementById("deleteAccountButton");
    const closeDeleteAccountPopup = document.getElementById("closeDeleteAccountPopup");
    const deleteAccountPopup = document.getElementById("deleteAccountPopup");

    const openPopup = (popup) => {
        popup.classList.remove("hidden");
        document.body.style.overflow = "hidden";
    };

    const closePopup = (popup) => {
        popup.classList.add("hidden");
        document.body.style.overflow = "";
    };

    if (settingsButton && closeSettingsPopup) {
        settingsButton.addEventListener("click", () => openPopup(settingsPopup));
        closeSettingsPopup.addEventListener("click", () => closePopup(settingsPopup));
    }

    if (updateAccountButton && closeUpdateAccountPopup) {
        updateAccountButton.addEventListener("click", () => openPopup(updateAccountPopup));
        closeUpdateAccountPopup.addEventListener("click", () => closePopup(updateAccountPopup));
    }

    if (deleteAccountButton && closeDeleteAccountPopup) {
        deleteAccountButton.addEventListener("click", () => openPopup(deleteAccountPopup));
        closeDeleteAccountPopup.addEventListener("click", () => closePopup(deleteAccountPopup));
    }

    ////////////////////////////////////////////////////////////
    // Четене на точките от маршрута
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
                        clearInterval(intervalId);
                        if (marker) {
                            map.removeLayer(marker);
                        }
                        toggleDeviceViewMode();
                        return;
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
            toggleDeviceViewMode();
        }
    });

    function toggleDeviceViewMode() {
        if (window.location.pathname === "/Map/MapTrackerLayout") {
            var deviceConnectionForm = document.getElementById('deviceConnectionForm');
            var deviceInfoView = document.getElementById('deviceInfoView');

            if (sessionStorage.getItem('connectedDeviceName') != null) {
                document.getElementById("connectedDeviceName").innerText = sessionStorage.getItem('connectedDeviceName');
                deviceConnectionForm.classList.remove("d-md-inline-flex");
                deviceConnectionForm.classList.add("d-none");
                deviceInfoView.classList.add("d-md-inline-flex");
                deviceInfoView.classList.remove("d-none");
            } else {
                deviceInfoView.classList.remove("d-md-inline-flex");
                deviceInfoView.classList.add("d-none");
                deviceConnectionForm.classList.add("d-md-inline-flex");
                deviceConnectionForm.classList.remove("d-none");
            }
        }
    }
});
