google.charts.load('current', { 'packages': ['bar'] });
google.charts.setOnLoadCallback(drawChart);


function drawChart() {

    var options = {
        chart: {
            title: 'Попит на саджанці',
            subtitle: 'Кількість саджанців, які придбали за весь час.',
        }
    };

    var statistics = [];
    $.ajax({
        type: "GET",
        url: "/Admin_Home/GetSeedlingsRates",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            statistics = result;
            console.log(statistics);
        },
        complete: function () {
            //data = google.visualization.arrayToDataTable([


            //for (var i = 0; i < statistics.length; i++) {
            var data = google.visualization.arrayToDataTable([
                ['Назва', 'Придбано одиниць'],
                statistics.forEach(s => { return [s.seedling, s.sum] })
                   // [statistics.forEach(s => s.seedling ), statistics.forEach(s => s.sum )]
               ]);

            var chart = new google.charts.Bar(document.querySelector('.columnchart_purchases'));

            chart.draw(data, google.charts.Bar.convertOptions(options));
        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.status + ': ' + xhr.statusText
            alert('Error - ' + errorMessage);
        }
    })
}