var treesAndUsers = [];

google.charts.load("current", { packages: ["corechart"] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {

    var data = google.visualization.arrayToDataTable([
        ['Trees', 'Plant'],
        ['Посаджено', parseInt(treesAndUsers.trees)],
        ['Залишилось посадити', parseInt(treesAndUsers.remainingTrees)]
    ]);

    var options = {
        colors: ['819F00', 'ABCD2D'],
        pieSliceTextStyle: {
            color: 'white',
        },
        pieHole: 0.3,
        fontSize: 18,
        select: null,
        tooltip: { isHtml: true, trigger: 'selection' }
        /*tooltip: { trigger: 'none' }*/,
        legend: 'none',
        pieSliceText: 'value',
        backgroundColor: 'none',
        chartArea: { left: 20, top: 10, width: '100%', height: '90%' },
    };

    var chart = new google.visualization.PieChart(document.getElementById('donut_single'));
    chart.draw(data, options);
}

$.ajax({
    type: "GET",
    url: "Home/GetDataForChart",
    contentType: "application/json",
    dataType: "json",
    success: function (result) {
        treesAndUsers = result;
    },
    complete: function () {
        document.getElementById("usersSupportedDiv").innerHTML += ` ${treesAndUsers.users} людей підтримало`;
        document.getElementById("treesPlantedDiv").innerHTML += ` ${treesAndUsers.trees} дерев посаджено`;
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
})