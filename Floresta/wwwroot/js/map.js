var markers = [];

$.ajax({
    type: "GET",
    url: "Map/GetMarkers",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        markers = result;
    }
})
var titleEl = document.getElementById('title');
var latEl = document.getElementById('lat');
var lngEl = document.getElementById('lng');
function initMap() {
    var uluru = { lat: 48.5405822, lng: 24.9988393 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 12,
        center: uluru,
        zoomControlOption: {
            position: google.maps.ControlPosition.LEFT_BOTTOM
        },
    });
    var input = document.getElementById('pac-input');
    var button = document.getElementById('searchid');
    var dropMarkers = document.getElementById("dropMarkers");
    var searchBox = new google.maps.places.SearchBox(input);


    //Pushing control on the map
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(input);
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(button);

    document.getElementById('searchid').onclick = function () {
        google.maps.event.trigger(input, 'focus')
        google.maps.event.trigger(input, 'keyword', { keyCode: 13 });
    };
    var searchMarkers = [];
    // Listern for the event fired when the user selecets a predition and retrieve more details
    searchBox.addListener('places_changed', function () {
        var places = searchBox.getPlaces();
        if (places.length == 0) {
            return;
        }
        //clear out the old markers
        searchMarkers.forEach(function (marker) {
            marker.setMap(null);
        });
        searchMarkers = [];
        //for each place, get the icon, name and location.
        var bounds = new google.maps.LatLngBounds();
        //debugger
        places.forEach(function (place) {
            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }
            var icon = {
                url: place.icon,
                size: new google.maps.Size(71, 71),
                origin: new google.maps.Point(0, 0),
                anchor: new google.maps.Point(17, 34),
                scaledSize: new google.maps.Size(25, 25),
            };
            //creates a marker for each place
            searchMarkers.push(new google.maps.Marker({
                map: map,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            if (place.geometry.viewport) {
                bounds.union(place.geometry.viewport);
            }
            else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);

    });

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng, "Click title");

    });


    function handleEvent(event) {
        document.getElementById('lat').value = event.latLng.lat();
        document.getElementById('lng').value = event.latLng.lng();
    }

    var marker = null;
    function placeMarker(location, title) {

        marker = new google.maps.Marker({
            position: location,
            animation: google.maps.Animation.DROP,
            title: title,
            draggable: true
        });
        // show info window
        //const infowindow = new google.maps.InfoWindow();
        //marker.addListener("click", () => {
        //    infowindow.open(markers.get("map"), markers);
        //});

        lngEl.value = marker.getPosition().lng();
        latEl.value = marker.getPosition().lat();
        marker.addListener('drag', handleEvent);
        marker.addListener('dragend', handleEvent);
        marker.setMap(map);
    }
    ///get markers from database
    dropMarkers.addEventListener("click", drop);
    function drop() {
        for (let i = 0; i < markers.length; ++i) {
            const marker = new google.maps.Marker({
                position: {
                    lat: parseFloat(markers[i].lat),
                    lng: parseFloat(markers[i].lng),
                },
                map: map,
                id: markers[i].id,
            });
            info(marker, markers[i].title);
        }
    }
    function info(marker, title) {
        const infowindow = new google.maps.InfoWindow({
            content: title,
        });
        marker.addListener("click", () => {
            
            infowindow.open(marker.get("map"), marker);
            titleEl.value = infowindow.content;
        });
    }

        //drop markers 
        // dropMarkers.addEventListener("click", drop);
        //for (let i = 0; i < markers.length; ++i) {
        //    const marker = new google.maps.Marker({
        //        position: {
        //            lat: parseFloat(markers[i].lat),
        //            lng: parseFloat(markers[i].lng),
        //        },
        //        map: map,
        //    });
        //function drop() {
        //    for (var i = 0; i < markers.length; i++) {

        //        placeMarker({ lat: parseFloat(markers[i].lat), lng: parseFloat(markers[i].lng) }, markers[i].title);
        //        console.log("asdf");
        //    }
        //}
}

// Attaches an info window to a marker with the provided message. When the
// marker is clicked, the info window will open with the secret message.
