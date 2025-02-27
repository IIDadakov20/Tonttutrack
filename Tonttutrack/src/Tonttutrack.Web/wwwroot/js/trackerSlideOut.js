document.addEventListener("DOMContentLoaded", function () {
    const toggleMenu = document.getElementById("toggle-menu");
    const slideOut = document.getElementById("tracker-menu");
    const togglePassword = document.getElementById("togglePassword");
    const passwordInput = document.querySelector("input[type='password']");
    const connectButton = document.getElementById("connect-btn");
    const disconnectButton = document.getElementById("disconnectButton");
    const deviceConnectionForm = document.getElementById("deviceConnectionForm");
    const deviceInfoView = document.getElementById("deviceInfoView");

    let isRecording = false;

    // Отваряне и затваряне на менюто
    toggleMenu.addEventListener("click", function () {
        slideOut.classList.toggle("open");
        toggleMenu.classList.toggle("open");
        toggleMenu.classList.toggle("active"); // Добавяме/премахваме класа
    });

// При натискане на Connect бутон
    connectButton.addEventListener("click", function (event) {
        event.preventDefault(); // Предотвратява изпращането на формата
        deviceConnectionForm.classList.add("hidden"); // Скрива формата за връзка
        deviceInfoView.classList.remove("hidden"); // Показва информацията за устройството
        deviceInfoView.classList.add("visible"); // Добавя видимост за информацията

        document.getElementById("connectedDeviceName").innerText = sessionStorage.getItem('connectedDeviceName');
    });

// При натискане на Disconnect бутон
    disconnectButton.addEventListener("click", function () {
        deviceInfoView.classList.add("hidden"); // Скрива информацията за устройството
        deviceConnectionForm.classList.remove("hidden"); // Показва формата за връзка
        deviceConnectionForm.classList.add("visible"); // Добавя видимост за формата
    });
});

// JavaScript част
document.addEventListener("DOMContentLoaded", function () {
    const routeList = document.getElementById("route-list");
    const routeStatistics = document.getElementById("route-statistics");
    const routeNameInput = document.getElementById("route-name-input");
    const saveRouteName = document.getElementById("save-route-name");
    const deleteRoute = document.getElementById("delete-route");

    let routes = [
        {name: "Route 1", date: "24/02/2025", recording: false},
        {name: "Route 2", date: "25/02/2025", recording: false}
    ];
    let activeRouteIndex = null;

    // Зареждане на маршрутите в списъка
    function loadRoutes() {
        routeList.innerHTML = ""; // Изчистване на текущия списък
        routes.forEach((route, index) => {
            const li = document.createElement("li");
            li.classList.add("route-item");
            if (activeRouteIndex === index) li.classList.add("active");

            li.innerHTML =`
                <div class="route-info">
                    <span class="route-name" data-index="${index}">${route.name}</span>
                    <span class="route-date">${route.date}</span>
                </div>
                <span class="route-toggle material-icons" data-index="${index}">
                    ${route.recording ? "stop" : "play_arrow"}
                </span>
            `;

            // Клик върху маршрут
            li.addEventListener("click", function () {
                if (activeRouteIndex === index) {
                    // Ако маршрутът вече е активен, деактивирай го
                    activeRouteIndex = null;
                    li.classList.remove("active");
                    routeStatistics.classList.add("hidden");
                } else {
                    // Деактивирай предишния активен маршрут
                    if (activeRouteIndex !== null) {
                        const prevActiveRoute = routeList.children[activeRouteIndex];
                        prevActiveRoute.classList.remove("active");
                    }

                    // Активирай текущия маршрут
                    activeRouteIndex = index;
                    li.classList.add("active");
                    showStatistics(route); // Покажи статистиките за маршрута
                }
            });

            routeList.appendChild(li); // Добави маршрута в списъка
        });
    }

    // Показване на статистиките за избрания маршрут
    function showStatistics(route) {
        routeStatistics.classList.remove("hidden"); // Покажи секцията със статистики
        routeNameInput.value = route.name; // Зареди името на маршрута в input полето
    }

    // Запазване на новото име на маршрута
    saveRouteName.addEventListener("click", function () {
        if (routeNameInput.value.trim() !== "" && activeRouteIndex !== null) {
            routes[activeRouteIndex].name = routeNameInput.value; // Обнови името на маршрута
            loadRoutes(); // Презареди списъка с маршрути
        }
    });

    // Изтриване на маршрут
    deleteRoute.addEventListener("click", function () {
        if (activeRouteIndex !== null) {
            routes.splice(activeRouteIndex, 1); // Премахни маршрута от списъка
            activeRouteIndex = null; // Нулирай активния индекс
            routeStatistics.classList.add("hidden"); // Скрий статистиките
            loadRoutes(); // Презареди списъка с маршрути
        }
    });

    // Зареди маршрутите при първоначално зареждане на страницата
    loadRoutes();
});

document.addEventListener("DOMContentLoaded", function () {
    const toggleViewBtn = document.getElementById("toggle-view-btn");
    const connectionSection = document.getElementById("connection-section");
    const routesSection = document.getElementById("routes-section");

    connectionSection.classList.remove("hidden");
    routesSection.classList.remove("visible");

    toggleViewBtn.addEventListener("click", function () {
        const currentLang = localStorage.getItem("selectedLanguage") || "en"; // Вземи текущия език

        if (toggleViewBtn.textContent === translations[currentLang].tracker.viewSavedRoutes) {
            toggleViewBtn.textContent = translations[currentLang].tracker.viewLiveTracking;
            connectionSection.classList.add("hidden");
            routesSection.classList.add("visible");
        } else {
            toggleViewBtn.textContent = translations[currentLang].tracker.viewSavedRoutes;
            connectionSection.classList.remove("hidden");
            routesSection.classList.remove("visible");
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    // Селектиране на бутона за запис
    const recordButton = document.getElementById("record-btn");

    // Проверка дали бутонът съществува в DOM-а
    if (recordButton) {
        let isRecording = false; // Състояние на записа

        // Функция за актуализиране на текста на бутона
        function updateRecordButtonText() {
            const currentLang = localStorage.getItem("selectedLanguage") || "en"; // Вземане на текущия език
            const buttonText = isRecording
                ? translations[currentLang].tracker.saveRecording
                : translations[currentLang].tracker.startRecording;
            recordButton.textContent = buttonText; // Актуализиране на текста
        }

        // Превключване между "Start Recording" и "Save Recording" при кликване
        recordButton.addEventListener("click", function () {
            isRecording = !isRecording; // Превключване на състоянието
            updateRecordButtonText(); // Актуализиране на текста
        });

        // Актуализиране на текста на бутона при смяна на езика
        const languageOptions = document.querySelectorAll('.language-option input[type="radio"]');
        languageOptions.forEach(option => {
            option.addEventListener("change", function () {
                updateRecordButtonText(); // Актуализиране на текста при смяна на езика
            });
        });

        // Инициализиране на текста на бутона при зареждане на страницата
        updateRecordButtonText();
    } else {
        console.warn("Бутонът за запис (record-btn) не е намерен в DOM-а.");
    }
});