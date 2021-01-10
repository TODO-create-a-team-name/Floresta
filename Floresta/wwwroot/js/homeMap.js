var markers = [];

let map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 48.9215, lng: 24.7097 },
        zoom: 11,
    });
}

$.ajax({
    type: "GET",
    url: "Map/GetMarkers",
    contentType: "application/json",    
    dataType: "json",
    success: function (result) {
        markers = result;
    },
    complete: function () {
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
                window.location.href = "/Map";
            });
        }
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
});

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
