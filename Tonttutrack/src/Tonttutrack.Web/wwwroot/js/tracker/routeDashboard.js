let routes = [];
let activeRoute = null;
let currentPage = 1;
const routesPerPage = 3;

$(function () {
    fetchUserRoutesNumber();
    $("#connection-section").addClass("visible");
    $("#routes-section").addClass("hidden");
    setupPaginationControls();
});

$("#toggle-view-btn").on("click", function () {

    const currentLang = localStorage.getItem("selectedLanguage") || "en";
    const savedRoutesText = translations[currentLang].tracker.viewSavedRoutes;
    const liveTrackingText = translations[currentLang].tracker.viewLiveTracking;

    const isSavedRoutes = $("#toggle-view-btn").text() === savedRoutesText;

    $("#toggle-view-btn").text(isSavedRoutes ? liveTrackingText : savedRoutesText);

    $("#connection-section").toggleClass("hidden", isSavedRoutes).toggleClass("visible", !isSavedRoutes);
    $("#routes-section").toggleClass("visible", isSavedRoutes).toggleClass("hidden", !isSavedRoutes);

    fetchAndRenderRoutes();
});

function setupPaginationControls() {
    $("#prev-page").on("click", function () {
        if (currentPage > 1) {
            currentPage--;
            fetchAndRenderRoutes();
        }
    });

    $("#next-page").on("click", function () {
        if (currentPage < Math.ceil(sessionStorage.getItem('totalRoutes') / routesPerPage)) {
            currentPage++;
            fetchAndRenderRoutes();
        }
    });
}

function fetchAndRenderRoutes() {
    fetchUserRoutes(currentPage).then(fetchedRoutes => {
        fetchedRoutes.forEach(route => {
            if (!routes.some(r => r.id === route.id)) {
                routes.push(route);
            }
        });
        renderRoutes();
    });
}

function renderRoutes() {
    const routeList = $("#route-list");
    routeList.empty();

    const start = (currentPage - 1) * routesPerPage;
    const end = start + routesPerPage;
    const paginatedRoutes = routes.slice(start, end);

    $.each(paginatedRoutes, function (index, route) {
        const li = createRouteListItem(route, start + index);
        routeList.append(li);
    });

    updatePaginationControls();
}

function createRouteListItem(route, index) {
    const li = $("<li>").addClass("route-item");

    if (route.id === activeRoute) {
        li.addClass("active");
    }

    li.html(`
        <div class="route-info">
            <span class="route-name" data-index="${index}">${route.name}</span>
            <span class="route-date">${route.date}</span>
        </div>
        <span class="route-toggle material-icons" data-index="${index}">
            ${route.recording ? "stop" : "play_arrow"}
        </span>
    `);

    li.on("click", function () {
        handleRouteClick(route, li);
    });

    return li;
}

function handleRouteClick(route, listItem) {
    const routeList = $("#route-list");

    if (route.id === activeRoute) {
        activeRoute = null;
        listItem.removeClass("active");
        $("#route-statistics").addClass("hidden");
    }
    else {
        if (activeRoute !== null) {
            routeList.find(".route-item.active").removeClass("active");
        }
        activeRoute = route.id;
        listItem.addClass("active");
        showStatistics(route);
    }
}

function showStatistics(route) {
    $("#route-statistics").removeClass("hidden");
    $("#route-name-input").val(route.name);
}

function updatePaginationControls() {
    const totalPages = Math.ceil(sessionStorage.getItem('totalRoutes') / routesPerPage);
    $("#page-indicator").text(`${currentPage} / ${totalPages}`);
    $("#prev-page").prop("disabled", currentPage === 1);
    $("#next-page").prop("disabled", currentPage === totalPages);
}
