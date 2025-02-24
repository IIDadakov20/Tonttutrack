document.addEventListener("DOMContentLoaded", function () {
    const toggleMenu = document.getElementById("toggle-menu");
    const slideOut = document.getElementById("tracker-menu");
    const recordButton = document.getElementById("record-btn");
    const togglePassword = document.getElementById("togglePassword");
    const passwordInput = document.querySelector("input[type='password']");

    let isRecording = false;

    // Отваряне и затваряне на менюто
    toggleMenu.addEventListener("click", function () {
        slideOut.classList.toggle("open");
        toggleMenu.classList.toggle("open");
        toggleMenu.classList.toggle("active"); // Добавяме/премахваме класа
    });
    
    // Превключване между "Start Recording" и "Save Recording"
    recordButton.addEventListener("click", function () {
        isRecording = !isRecording;
        recordButton.textContent = isRecording ? "Save Recording" : "Start Recording";
    });

    // Показване и скриване на паролата
    togglePassword.addEventListener("change", function () {
        passwordInput.type = this.checked ? "text" : "password";
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
        { name: "Route 1", date: "24/02/2025", recording: false },
        { name: "Route 2", date: "25/02/2025", recording: false }
    ];
    let activeRouteIndex = null;

    // Зареждане на маршрутите в списъка
    function loadRoutes() {
        routeList.innerHTML = ""; // Изчистване на текущия списък
        routes.forEach((route, index) => {
            const li = document.createElement("li");
            li.classList.add("route-item");
            if (activeRouteIndex === index) li.classList.add("active");

            li.innerHTML = `
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

    // Начално състояние: показване на секцията за свързване
    connectionSection.classList.remove("hidden");
    routesSection.classList.remove("visible");

    // Превключване между двата режима
    toggleViewBtn.addEventListener("click", function () {
        if (toggleViewBtn.textContent === "View Saved Routes") {
            // Превключване към режим "View Live Tracking"
            toggleViewBtn.textContent = "View Live Tracking";
            connectionSection.classList.add("hidden");
            routesSection.classList.add("visible");
        } else {
            // Превключване към режим "View Saved Routes"
            toggleViewBtn.textContent = "View Saved Routes";
            connectionSection.classList.remove("hidden");
            routesSection.classList.remove("visible");
        }
    });
});