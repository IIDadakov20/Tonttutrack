const accessToken = '6mjPfGcWuPzKz5cthMD97rjbuF6t4xlNBj7megquuKGRcQYSPHK0Ay0cTqrHaASd';
const map = L.map('map').setView([42.448284, 27.076411], 10);
const styles = ['sunny', 'dark'];
const baselayers = {};

styles.forEach((style) =>
    baselayers[style] = L.tileLayer(
        `https://tile.jawg.io/${style}/{z}/{x}/{y}.png?lang=en&access-token=${accessToken}`, {
        maxZoom: 16,
        minZoom: 3
    })
);

var southWest = L.latLng(-80, -200),
    northEast = L.latLng(84.5, 200)
var bounds = L.latLngBounds(southWest, northEast);

map.setMaxBounds(bounds);
map.on('drag', function () {
    map.panInsideBounds(bounds, { animate: true });
});

baselayers['sunny'].addTo(map);
L.control.layers(baselayers).addTo(map);