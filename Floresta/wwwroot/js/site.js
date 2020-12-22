
    var menu = document.querySelector('.nav_menu'),
        icon = document.querySelector('.animated_menu_icon'),
        menuButton = document.querySelector('#menu_button_container'),
        open= false;

menuButton.onclick = function () {
    if (open == false) {
        icon.classList.add('menu-opened');
        menu.classList.add('menu-opened');
        open = !open;
    }
    else {
        menu.classList.add('menu-closed');
        icon.classList.remove('menu-opened');
        menu.classList.remove('menu-opened');
        setTimeout(closeMenu, 1000)
        open = !open;
    }
    function closeMenu() {
        menu.classList.remove('menu-closed');
    }   
}

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
        tooltip: null,
        fontSize: 16,
        legend: 'none',
        pieSliceText: 'value',
        backgroundColor: 'none',
        tooltip: 'nope',
        chartArea: {left:20,top:0,width:'100%',height:'100%'},
    };


    var chart = new google.visualization.PieChart(document.getElementById('donut_single'));
    chart.draw(data, options);
}
