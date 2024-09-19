function initializeMap(latitude, longitude){
    // Creating the map.    
    window.map = new ymaps.Map("map", {
        // The map center coordinates.
        // Default order: «latitude, longitude».
        // To not manually determine the map center coordinates,
        // use the Coordinate detection tool.
        center: [latitude, longitude],
        // Zoom level. Acceptable values:
        // from 0 (the entire world) to 19.
        zoom: 10
    });
}

function addPolygonOnMap(coordinates, name, id, dotNetHelper) {
    if (window.map){
        const polygon = new ymaps.Polygon([
            coordinates
        ], {
            hintContent: name
        }, {
            fillColor: '#EE1E23',
            fillOpacity: 0.4,
            strokeWidth: 0
        });

        polygon.events.add('click', function (e) {
            dotNetHelper.invokeMethodAsync('OnPolygonClicked', id);
        });

        window.map.geoObjects.add(polygon);
    }
}