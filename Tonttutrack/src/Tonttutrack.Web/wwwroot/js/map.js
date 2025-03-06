const accessToken = '6mjPfGcWuPzKz5cthMD97rjbuF6t4xlNBj7megquuKGRcQYSPHK0Ay0cTqrHaASd';
const map = L.map('map', {attributionControl: false}).setView([42.448284, 27.076411], 10);
const styles = ['sunny', 'dark'];
const baselayers = {};

// Създаваме слоевете за sunny и dark
styles.forEach((style) =>
    baselayers[style] = L.tileLayer(
        `https://tile.jawg.io/${style}/{z}/{x}/{y}.png?lang=en&access-token=${accessToken}`, {
            maxZoom: 17,
            minZoom: 3
        })
);

var southWest = L.latLng(-80, -200),
    northEast = L.latLng(84.5, 200);
var bounds = L.latLngBounds(southWest, northEast);

map.setMaxBounds(bounds);
map.on('drag', function () {
    map.panInsideBounds(bounds, { animate: true });
});

// Начално добавяне на слой (sunny)
baselayers['sunny'].addTo(map);
L.control.layers(baselayers).addTo(map);

// Добавяме event listener за смяна на темата
const themeToggle = document.getElementById('themeToggle');
const savedTheme = localStorage.getItem('theme');

// Проверяваме текущата тема и добавяме съответния слой
if (savedTheme === 'dark') {
    baselayers['dark'].addTo(map);
    baselayers['sunny'].removeFrom(map);
    themeToggle.checked = true;
} else {
    baselayers['sunny'].addTo(map);
    baselayers['dark'].removeFrom(map);
}

// Слушател за промяна на toggle бутона
themeToggle.addEventListener('change', function() {
    if (this.checked) {
        baselayers['dark'].addTo(map);  // Превключваме към dark слой
        baselayers['sunny'].removeFrom(map);
        localStorage.setItem('theme', 'dark');
        enableDarkTheme()
    } else {
        baselayers['sunny'].addTo(map);  // Превключваме към sunny слой
        baselayers['dark'].removeFrom(map);
        localStorage.setItem('theme', 'light');
        disableDarkTheme()
    }
});
