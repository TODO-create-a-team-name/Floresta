

//mobile menu triggers
    var menu = document.querySelector('.nav_menu'),
        icon = document.querySelector('.animated_menu_icon'),
        menuButton = document.querySelector('#menu_button_container'),
        open= false;

menuButton.addEventListener('click', {
    handleEvent(event) {
        if (open == false) {
            icon.classList.add('menu-opened');
            menu.classList.add('menu-opened');
            open = !open;
        }
        else {
            menu.classList.add('menu-closed');
            icon.classList.remove('menu-opened');
            menu.classList.remove('menu-opened');
            setTimeout(closeMenu, 490)
        }
        function closeMenu() {
            menu.classList.remove('menu-closed');
            open = !open;
        }
    }
});


google.charts.load("current", { packages: ["corechart"] });
google.charts.setOnLoadCallback(drawChart);
function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Trees', 'Plant'],
        ['Посаджено', 1220],
        ['Залишилось посадити', 5780]     
    ]);

    var options = {
        colors: ['819F00', 'ABCD2D'],
        pieSliceTextStyle: {
            color: 'white',
        },
        fontSize: 18,
        legend: 'none',
        pieSliceText: 'value',
        backgroundColor: 'none',
        tooltip: { trigger: 'none' },
        chartArea: {left:20,top:0,width:'100%',height:'100%'},
    };


    var chart = new google.visualization.PieChart(document.getElementById('donut_single'));
    chart.draw(data, options);
}




// home map
let map;
function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 48.9215, lng: 24.7097 },
        zoom: 11,
    });
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