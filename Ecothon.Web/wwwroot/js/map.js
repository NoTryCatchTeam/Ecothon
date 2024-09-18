async function initializeMap() {
    // The `ymaps3.ready` promise will be resolved when all the API components are loaded
    await ymaps3.ready;
    
    const {YMap, YMapDefaultSchemeLayer} = ymaps3;

    // Map creation
    window.map = new YMap(
        // Pass the link to the HTMLElement of the container
        document.getElementById('map'),

        // Pass the initialization parameters
        {
            location: {
                // The map center coordinates
                center: [37.62, 55.75],

                // Zoom level
                zoom: 10
            }
        }
    );

    window.map.addChild(new YMapDefaultSchemeLayer({
        customization: [
            {
                "tags": {
                    "any": [
                        "park",
                        "cemetery"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "vegetation"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "industrial",
                        "construction_site",
                        "medical",
                        "sports_ground",
                        "beach"
                    ]
                },
                "types": "polygon",
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": "transit",
                    "none": [
                        "transit_location",
                        "transit_line",
                        "transit_schema",
                        "is_unclassified_transit"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": "urban_area",
                    "none": [
                        "residential",
                        "industrial",
                        "cemetery",
                        "park",
                        "medical",
                        "sports_ground",
                        "beach",
                        "construction_site"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "transit"
                    ]
                },
                "elements": [
                    "label.icon",
                    "label.text"
                ],
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "outdoor",
                        "park",
                        "cemetery",
                        "medical"
                    ]
                },
                "elements": "label",
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": "poi",
                    "none": [
                        "outdoor",
                        "park",
                        "cemetery",
                        "medical"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": "road"
                },
                "types": "point",
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "food_and_drink",
                        "shopping",
                        "commercial_services"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "traffic_light"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "entrance"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "road"
                    ],
                    "none": [
                        "road_1",
                        "road_2",
                        "road_3",
                        "road_4",
                        "road_5",
                        "road_6",
                        "road_7"
                    ]
                },
                "elements": "label.icon",
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "building",
                        "address",
                        "fence"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            },
            {
                "tags": {
                    "any": [
                        "transit"
                    ]
                },
                "stylers": {
                    "visibility": "off"
                }
            }
        ]
    }));
}