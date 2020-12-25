var data = {};

const iconBase = "http://maps.google.com/mapfiles/kml/paddle/";
const icons = {
    finish: {
        icon: iconBase + "grn-circle.png",
    },
    working: {
        icon: iconBase + "red-circle.png",
    },
};

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
            var icon = icons.finish;
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
        placeMarker(event.latLng, "New marker");

    });

    function handleEvent(event) {
        document.getElementById('lat').value = event.latLng.lat();
        document.getElementById('lng').value = event.latLng.lng();
    }
    var marker = null;
    function placeMarker(location, title) {
        if (data.isAdmin) {
            marker = new google.maps.Marker({
                position: location,
                animation: google.maps.Animation.DROP,
                title: title,
                draggable: true
            });
            $("#lng").val(marker.getPosition().lng());
            $("#lat").val(marker.getPosition().lat());
            marker.addListener('drag', handleEvent);
            marker.addListener('dragend', handleEvent);
            marker.setMap(map);
        }

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

function checkType(isFinish) {
    if (isFinish)
        return "finish";
    else
        return "working"
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

        for (var i = 0; i < data.seedlings.length; i++) {
            if (data.seedlings[i].id == seedlingId) {
                seedling = data.seedlings[i];
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

$.ajax({
    type: "GET",
    url: "Map/GetRequiredData",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        data.markers = result.markers;
        data.seedlings = result.seedlings;
        data.isAdmin = result.isAdmin;
    },
    complete: function () {
        for (var i = 0; i < data.markers.length; i++) {
            const marker = new google.maps.Marker({
                position: {
                    lat: parseFloat(data.markers[i].lat),
                    lng: parseFloat(data.markers[i].lng),

                },
                map: map,
                title: data.markers[i].title,
                plantCount: data.markers[i].plantCount,
                id: data.markers[i].id,
                animation: google.maps.Animation.DROP,
                icon: icons[checkType(data.markers[i].isPlantingFinished)].icon
            });
            if (!data.markers[i].isPlantingFinished) {
                marker.addListener("click", () => {
                    if (marker.getAnimation() !== null) {
                        marker.setAnimation(null);
                    }
                    else {
                        marker.setAnimation(google.maps.Animation.BOUNCE);
                    }
                });
                info(marker, data.markers[i].title + "<br> plantCount: " + data.markers[i].plantCount);
            }
        }
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
});