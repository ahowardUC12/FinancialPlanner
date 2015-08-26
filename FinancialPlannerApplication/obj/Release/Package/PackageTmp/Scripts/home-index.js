google.load('visualization', '1.0', { 'packages': ['corechart'] });

$(document).ready(function () {
    $(".datepicker").datepicker({ autoclose: true });

    var urlAccount = $("#accountChartButton").data("request-url");

    var getBudgetCharts = function () {
        var selectedBudgetId = $("#SelectedBudgetId").val() == undefined ? 0 : $("#SelectedBudgetId").val();

        var url = $("#budgetChartHeader").data("request-url");
        var urlItem = $("#budgetChartHeader").data("request-url-item");

        var getDataUrl = url + "?budgetId=" + selectedBudgetId;
        var getItemDataUrl = urlItem + "?budgetId=" + selectedBudgetId;

        $.ajax({
            url: getDataUrl,
            type: "get",
            success: function (response) {
                if(response.length > 0)
                    google.setOnLoadCallback(drawChart(response));
            }
        });

        $.ajax({
            url: getItemDataUrl,
            type: "get",
            success: function (response) {
                if (response.length > 0)
                    google.setOnLoadCallback(drawChartItem(response));
            }
        });
    };

    var getBudgetItemProgress = function () {
        var selectedBudgetId = $("#SelectedBudgetId").val() == undefined ? 0 : $("#SelectedBudgetId").val();
        var url = $("#budgetProgress").data("request-url");

        $.ajax({
            url: url + "?budgetId=" + selectedBudgetId,
            type: "get",
            success: function (response) {
                var div = $("#budgetProgress");
                $.each(response, function (idx, elem) {
                    div.append("<h5>" + elem.Name + "</h5>");
                    div.append("<div class='progress'><div class='progress-bar progress-bar-success' style='width:" + elem.AmountSpentPerc + "%'>" +
                        "$" + elem.AmountSpent + "</div>" + "<div class='progress-bar progress-bar-warning' style='width:" + elem.AmountLeftPerc + "%'>" +
                        "$" + elem.AmountLeft + "</div></div>");
                });
            }
        });
    };

    getBudgetCharts();
    getBudgetItemProgress();

    $("#SelectedBudgetId").on("change", function () {
        getBudgetCharts();
        var budgetProgress = $("#budgetProgress");
        budgetProgress.empty();
        getBudgetItemProgress();
    });

    $("#accountChartButton").on("click", function () {
        var selectedAccountId = $("#SelectedAccountId").val();
        var fromDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();

        var getAccountDataUrl = urlAccount + "?accountId=" + selectedAccountId + "&fromDate=" + fromDate + "&toDate=" + toDate;
        $.ajax({
           url: getAccountDataUrl,
           type: "get",
           success: function (response) {
               var ul = $("#acctErrors");
               ul.empty();

               if (response.length > 0) {
                   google.setOnLoadCallback(drawAccountChart(response));
                   $("#accountChartBalance").show();
                   $("#accountChartTransaction").show();
               } else {
                   var parsedTDate = Date.parse(toDate);
                   var parsedFDate = Date.parse(fromDate);
                   var hasErrors = false;

                   if (toDate === "" || fromDate === "") {
                       ul.append("<li>Both a transactions from date and transactions to date must be entered or selected</li>");
                       hasErrors = true;
                   }

                   if (parsedTDate < parsedFDate && toDate !== "" && fromDate !== "") {
                       ul.append("<li>Transactions from date must be later than transactions to date</li>");
                       hasErrors = true;
                   }

                   if (!hasErrors)
                       $("#noAccountData").show();
                   else 
                       $("#noAccountData").hide();
                   
                   $("#accountChartBalance").hide();
                   $("#accountChartTransaction").hide();
               }
                   
           }
       });
    });

    $("#accountChartButton").click();

    $("#accountInfoHide").on("click", function () {
        $("#accountSection").hide();
        $("#accountInfoShow").show();
        $("#accountInfoHide").hide();
        $("#accountErrors").hide();
    });

    $("#accountInfoShow").on("click", function () {
        $("#accountSection").show();
        $("#accountInfoShow").hide();
        $("#accountInfoHide").show();
        $("#accountErrors").show();
    });

    $("#budgetInfoHide").on("click", function () {
        $("#budgetSection").hide();
        $("#budgetInfoShow").show();
        $("#budgetInfoHide").hide();
    });

    $("#budgetInfoShow").on("click", function () {
        $("#budgetSection").show();
        $("#budgetInfoShow").hide();
        $("#budgetInfoHide").show();
    });

    $("#expenseInfoHide").on("click", function () {
        $("#expenseSection").hide();
        $("#expenseInfoShow").show();
        $("#expenseInfoHide").hide();
    });

    $("#expenseInfoShow").on("click", function () {
        $("#expenseSection").show();
        $("#expenseInfoShow").hide();
        $("#expenseInfoHide").show();
    });
});

var setData = function(data, budgetChartData) {
    for (var i = 0; i < budgetChartData.length; i++) {
        data.addRow([budgetChartData[i].Name, budgetChartData[i].Amount]);
    };
};

function drawChart(budgetChartData) {
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Budget Item');
    data.addColumn('number', 'Allotment Amount');

    setData(data, budgetChartData);

    var options = {
        'title': budgetChartData[0].BudgetName + " Budget Item Allotments",
        'width': 400,
        'height': 300
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechartBudget'));
    chart.draw(data, options);
};

function drawChartItem(budgetChartData) {
    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Budget Item');
    data.addColumn('number', 'Amount Spent');

    setData(data, budgetChartData);

    var options = {
        'title': budgetChartData[0].BudgetName + " Transactions",
        'width': 400,
        'height': 300
    };

    var chart = new google.visualization.PieChart(document.getElementById('piechartBudgetItem'));
    chart.draw(data, options);
};

var setAccountBalanceData = function(dataArray, accountData) {
    dataArray.push(['Date', 'Balance']);
    for (var j = 0; j < accountData.length; j++) {

        dataArray.push([new Date(accountData[j].Year, accountData[j].Month - 1, accountData[j].Day), accountData[j].Balance]);
    };
};

var setAccountTransactionData = function(dataArray, accountData) {
    for (var i = 0; i < accountData.length; i++) {
        dataArray.push([new Date(accountData[i].Year, accountData[i].Month - 1, accountData[i].Day), accountData[i].Amount]);
    };
};

var drawAccountTransactionChart = function(transactionArray) {
    var data = google.visualization.arrayToDataTable(
       transactionArray
   );

    var options= {
        title: 'Transaction totals by day',
        hAxis: { title: 'Date', titleTextStyle: { color: '#333' } },
        vAxis: { minValue: 0 },
        height: 300,
        width: 600
    };

    var chart = new google.visualization.AreaChart(document.getElementById('accountChartTransaction'));
    chart.draw(data, options);
};

var drawAccountBalanceChart = function (accountArray) {
    var data = google.visualization.arrayToDataTable(
        accountArray
    );

    var options = {
        title: 'Balance over time',
        hAxis: { title: 'Date', titleTextStyle: { color: '#333' } },
        vAxis: { minValue: 0 },
        height: 300,
        width: 600
    };

    var chart = new google.visualization.AreaChart(document.getElementById('accountChartBalance'));
    chart.draw(data, options); 
};

function drawAccountChart(accountData) {
    var transactionArray = new Array();
    transactionArray.push(['Date', 'Amount']);

    setAccountTransactionData(transactionArray, accountData);

    var accountArray = new Array();
    setAccountBalanceData(accountArray, accountData);

    drawAccountTransactionChart(transactionArray);
    drawAccountBalanceChart(accountArray);
}