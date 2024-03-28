mapboxgl.accessToken = 'pk.eyJ1IjoiYWJkLW9kZWgwMDciLCJhIjoiY2xzazQ1NTc0MmxzbDJqbm9iaHpwazhncSJ9.n56bne7ZI7yS5YqcS0VfQA';

const map = new mapboxgl.Map({
    container: 'map',
    center: [55.254699280403784, 25.133202126043326], //[lng, lat]
    zoom: 9,
    pitch: 62,
    bearing: -20
});

map.addControl(
    new MapboxGeocoder({
        accessToken: mapboxgl.accessToken,
        mapboxgl: mapboxgl,
    })
);


let geoJsonData;
let mainPoint;
let features; 

$.ajax({
    url: '/Profiles/GetGeoJson',
    type: 'GET',
    dataType: 'json',
    success: function (data) {
        geoJsonData = data;
        mainPoint = geoJsonData.features[0].geometry.coordinates[0]
        features = geoJsonData.features[1].geometry.coordinates

        console.log('GeoJSON data:', geoJsonData);
    },
    error: function (error) {
        console.error('Error loading GeoJSON data:', error);
    }
});

map.on("load", () => {

    map.flyTo({
        center: mainPoint
    });

    map.addSource("test", {
        type: "geojson",
        data: geoJsonData,
    });



    map.addLayer({
        type: "line",
        source: "test",
        id: "line-background",
        paint: {
            "line-color": "red",
            "line-width": 5,
            "line-opacity": 0.6,
        },
    });

    map.addLayer({
        type: "line",
        source: "test",
        id: "line-dashed",
        paint: {
            "line-color": "#0096FF",
            "line-width": 5,
            "line-dasharray": [0, 4, 3],
        },
    });

   
    const dashArraySequence = [
        [0, 4, 3],
        [0.5, 4, 2.5],
        [1, 4, 2],
        [1.5, 4, 1.5],
        [2, 4, 1],
        [2.5, 4, 0.5],
        [3, 4, 0],
        [0, 0.5, 3, 3.5],
        [0, 1, 3, 3],
        [0, 1.5, 3, 2.5],
        [0, 2, 3, 2],
        [0, 2.5, 3, 1.5],
        [0, 3, 3, 1],
        [0, 3.5, 3, 0.5],
    ];

    let step = 0;

    function animateDashArray(timestamp) {
        const newStep = parseInt((timestamp / 100) % dashArraySequence.length);

        if (newStep !== step) {
            map.setPaintProperty(
                "line-dashed",
                "line-dasharray",
                dashArraySequence[step]
            );
            step = newStep;
        }

        requestAnimationFrame(animateDashArray);
    }

    animateDashArray(0);

    console.log(features)

    for (let i = 1; i < features.length; i++) {

        const feature = features[i];

        const el = document.createElement('div');
        el.className = 'marker';

        new mapboxgl.Marker(el).setLngLat(feature).addTo(map);
    }

});



map.addControl(new mapboxgl.FullscreenControl());

