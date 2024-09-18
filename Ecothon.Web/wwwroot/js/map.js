function initializeMap(customization){ 
    // Creating the map.    
    window.map = new ymaps.Map("map", {
        // The map center coordinates.
        // Default order: «latitude, longitude».
        // To not manually determine the map center coordinates,
        // use the Coordinate detection tool.
        center: [55.75, 37.62],
        // Zoom level. Acceptable values:
        // from 0 (the entire world) to 19.
        zoom: 10
    });

    window.map.addChild(new YMapDefaultSchemeLayer({
        customization: JSON.parse(customization)
    }));
}