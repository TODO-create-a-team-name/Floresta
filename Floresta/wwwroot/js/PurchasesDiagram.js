$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "/Admin_Home/GetSeedlingsRates",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            var keys = Object.keys(result);
            var data = new Array();
            for (var i = 0; i < keys.length; i++) {
                var arr = new Array();
                arr.push(keys[i]);
                arr.push(result[keys[i]]);
                data.push(arr);
            }
            createCharts(data);
        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            alert('Error - ' + errorMessage);
        }
    })
})

function createCharts(data) {
    Highcharts.chart('columnchart_purchases', {
        chart: {
            type: 'column'
        },
        title: {
            text: "Статистика покупок"
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -45,
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Придбано'
            }
        },
        legend: {
            enabled: false
        },
        series: [{
            type: 'column',
            data: data,
        }]
    });
}
