google.charts.load('current', { 'packages': ['bar'] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Year', 'Sales', 'Expenses', 'Profit'],
        ['2019', 660, 1120, 300],
        ['2020', 1030, 540, 350]
    ]);

    var options = {
        chart: {
            title: 'Company Performance',
            subtitle: 'Sales, Expenses, and Profit: 2019-2020',
        }
    };

    var chart = new google.charts.Bar(document.querySelector('.columnchart_purchases'));

    chart.draw(data, google.charts.Bar.convertOptions(options));
}