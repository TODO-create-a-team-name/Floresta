var treesAndUsers = [];
$.ajax({
    type: "GET",
    url: "Home/GetDataForChart",
    contentType: "application/json",
    dataType: "json",
    async: false,
    success: function (result) {
        treesAndUsers = result;
    },
    error: function (xhr, status, error) {
        var errorMessage = xhr.status + ': ' + xhr.statusText
        alert('Error - ' + errorMessage);
    }
})

document.getElementById("usersSupportedDiv").innerHTML += ` ${treesAndUsers.users} людей підтримало`;
document.getElementById("treesPlantedDiv").innerHTML += ` ${treesAndUsers.trees} дерев посаджено`;

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
        fontSize: 18,
        tooltip: { trigger: 'none' },
        legend: 'none',
        pieSliceText: 'value',
        backgroundColor: 'none',
        chartArea: { left: 20, top: 0, width: '100%', height: '100%' },
    };

    var chart = new google.visualization.PieChart(document.getElementById('donut_single'));
    chart.draw(data, options);
}