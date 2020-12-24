var markers = [];
var seedlings = [];

$.ajax({
    async: false,
    type: "GET",
    url: "Map/GetMarkers",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        markers = result;
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
})

$.ajax({
    type: "GET",
    url: "Map/GetSeedlings",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        seedlings = result;
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
})

var map;
function initMap() {
    var uluru = { lat: 48.9215, lng: 24.7097};

     map = new google.maps.Map(document.getElementById('map'), {
        zoom: 11,
        center: uluru,
        zoomControlOption: {
            position: google.maps.ControlPosition.LEFT_BOTTOM
        },
    });

    var input = document.getElementById('pac-input');
    var searchBox = new google.maps.places.SearchBox(input);


    //Pushing control on the map
    map.controls[google.maps.ControlPosition.TOP_CENTER].push(input);
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

    for (var i = 0; i < markers.length; i++) {
        const marker = new google.maps.Marker({
            position: {
                lat: parseFloat(markers[i].lat),
                lng: parseFloat(markers[i].lng),
            },
            map: map,
            title: markers[i].title,
            plantCount: markers[i].plantCount,
            id: markers[i].id,
            animation: google.maps.Animation.DROP,
        });
        marker.addListener("click", () => {
            if (marker.getAnimation() !== null) {
                marker.setAnimation(null);
            }
            else {
                marker.setAnimation(google.maps.Animation.BOUNCE);
            }
        });
        info(marker, markers[i].title);
    }
   
    function info(marker, title) {
        const infowindow = new google.maps.InfoWindow({
            content: title,
        });
        marker.addListener("click", () => {

            infowindow.open(marker.get("map"), marker);
            $("#markerIdInput").val(marker.id);
            $("#markerTitleInput").val(marker.title);

            var seedlingId = $("#seedlingsDropdown option:selected").val();

            var seedling;

            for (var i = 0; i < seedlings.length; i++) {
                if (seedlings[i].id == seedlingId) {
                    seedling = seedlings[i];
                }
                break;
            }
            var count;
            if (seedling.amount >= marker.plantCount) {
                count = marker.plantCount;
            }
            else if (marker.plantCount > seedling.amount) {
                count = seedling.amount;
            }

            $("#plantCountInput").attr({
                "max": count,
                "min": 1
            });
        });
    }
}
var regionBt = document.querySelectorAll('.region_button');
regionBt.forEach(region => {
    region.addEventListener('click', {
        handleEvent(event) {
            regionBt.forEach(node => {// remove style for button 
                node.classList.remove('region_selected');
            });
            switch (region.innerHTML) {
                case 'Івано-Франківська': setRegoin(48.9215, 24.7097)
                    break;
                case 'Львівська': setRegoin(49.83826, 24.02324)
                    break;
                case 'Тернопільська': setRegoin(49.553516, 25.594767)
                    break;
            }
            function setRegoin(lat, lng) { //set new location
                region.classList.add('region_selected');
                const pos = { lat: lat, lng: lng, };
                map.setCenter(pos);
            }
        }
    });
});