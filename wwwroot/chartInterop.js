console.log("Before createChart function");

var createChart = function(chartId, chartData) {
    console.log("Creating chart with ID:", chartId);
    const ctx = document.getElementById(chartId).getContext("2d");
    new Chart(ctx, chartData);
    console.log("Chart created with ID:", chartId);
};