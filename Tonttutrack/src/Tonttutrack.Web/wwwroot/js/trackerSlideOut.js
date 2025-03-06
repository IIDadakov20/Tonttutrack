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
    const routeStatistics = document.getElementById("route-statistics");
    const routeNameInput = document.getElementById("route-name-input");
    const saveRouteName = document.getElementById("save-route-name");
    const deleteRoute = document.getElementById("delete-route");

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
    const routesPerPage = 3;

    saveRouteName.addEventListener("click", function () {
        if (routeNameInput.value.trim() !== "" && activeRouteName !== null) {
            const activeRoute = routes.find(route => route.name === activeRouteName);
            if (activeRoute) {
                activeRoute.name = routeNameInput.value;
                activeRouteName = activeRoute.name; // Актуализираме името на активния маршрут
                //loadRoutes();
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

            //loadRoutes();
        }
    });

    //loadRoutes();
});