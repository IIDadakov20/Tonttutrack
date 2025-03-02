document.addEventListener("DOMContentLoaded", function () {
    const toggleMenu = document.getElementById("toggle-menu");
    const slideOut = document.getElementById("tracker-menu");

    // Отваряне и затваряне на менюто
    toggleMenu.addEventListener("click", function () {
        slideOut.classList.toggle("open");
        toggleMenu.classList.toggle("open");
        toggleMenu.classList.toggle("active"); // Добавяме/премахваме класа
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const routeList = document.getElementById("route-list");
    const routeStatistics = document.getElementById("route-statistics");
    const routeNameInput = document.getElementById("route-name-input");
    const saveRouteName = document.getElementById("save-route-name");
    const deleteRoute = document.getElementById("delete-route");
    const prevPageButton = document.getElementById("prev-page");
    const nextPageButton = document.getElementById("next-page");
    const pageIndicator = document.getElementById("page-indicator");

    let routes = [
        {name: "Route 1", date: "24/02/2025", recording: false},
        {name: "Route 2", date: "25/02/2025", recording: false},
        {name: "Route 3", date: "26/02/2025", recording: false},
        {name: "Route 4", date: "27/02/2025", recording: false},
        {name: "Route 5", date: "28/02/2025", recording: false},
        {name: "Route 6", date: "01/03/2025", recording: false},
        {name: "Route 7", date: "01/03/2025", recording: false},
        {name: "Route 8", date: "01/03/2025", recording: false},
        {name: "Route 9", date: "01/03/2025", recording: false},
        {name: "Route 10", date: "01/03/2025", recording: false},
    ];
    let activeRouteName = null;
    let currentPage = 1;
    const routesPerPage = 4;

    function loadRoutes() {
        routeList.innerHTML = "";
        const start = (currentPage - 1) * routesPerPage;
        const end = start + routesPerPage;
        const paginatedRoutes = routes.slice(start, end);

        paginatedRoutes.forEach((route, index) => {
            const li = document.createElement("li");
            li.classList.add("route-item");
            if (route.name === activeRouteName) {
                li.classList.add("active");
            }

            li.innerHTML =`
                <div class="route-info">
                    <span class="route-name" data-index="${start + index}">${route.name}</span>
                    <span class="route-date">${route.date}</span>
                </div>
                <span class="route-toggle material-icons" data-index="${start + index}">
                    ${route.recording ? "stop" : "play_arrow"}
                </span>
            `;

            li.addEventListener("click", function () {
                if (route.name === activeRouteName) {
                    // Ако маршрутът вече е активен, деактивирай го
                    activeRouteName = null;
                    li.classList.remove("active");
                    routeStatistics.classList.add("hidden");
                } else {
                    // Деактивирай предишния активен маршрут
                    if (activeRouteName !== null) {
                        const prevActiveLi = routeList.querySelector(".route-item.active");
                        if (prevActiveLi) prevActiveLi.classList.remove("active");
                    }

                    // Активирай текущия маршрут
                    activeRouteName = route.name;
                    li.classList.add("active");
                    showStatistics(route);
                }
            });

            routeList.appendChild(li);
        });

        updatePaginationControls();
    }

    function showStatistics(route) {
        routeStatistics.classList.remove("hidden");
        routeNameInput.value = route.name;
    }

    function updatePaginationControls() {
        const totalPages = Math.ceil(routes.length / routesPerPage);
        pageIndicator.textContent = `${currentPage} / ${totalPages}`;
        prevPageButton.disabled = currentPage === 1;
        nextPageButton.disabled = currentPage === totalPages;
    }

    prevPageButton.addEventListener("click", function () {
        if (currentPage > 1) {
            currentPage--;
            loadRoutes();
        }
    });

    nextPageButton.addEventListener("click", function () {
        if (currentPage < Math.ceil(routes.length / routesPerPage)) {
            currentPage++;
            loadRoutes();
        }
    });

    saveRouteName.addEventListener("click", function () {
        if (routeNameInput.value.trim() !== "" && activeRouteName !== null) {
            const activeRoute = routes.find(route => route.name === activeRouteName);
            if (activeRoute) {
                activeRoute.name = routeNameInput.value;
                activeRouteName = activeRoute.name; // Актуализираме името на активния маршрут
                loadRoutes();
            }
        }
    });

    deleteRoute.addEventListener("click", function () {
        if (activeRouteName !== null) {
            routes = routes.filter(route => route.name !== activeRouteName);
            activeRouteName = null;
            routeStatistics.classList.add("hidden");

            // Проверка дали текущата страница е празна
            const totalPages = Math.ceil(routes.length / routesPerPage);
            if (currentPage > totalPages) {
                currentPage = totalPages; // Връщане към предишната страница
            }

            loadRoutes();
        }
    });

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