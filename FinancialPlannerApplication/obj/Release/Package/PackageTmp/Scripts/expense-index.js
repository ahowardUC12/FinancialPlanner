$(document).ready(function () {

    var expId = $("#SelectedExpenseId").val();

    
    var loadTransactions = function(id) {
        //var url = $(".expenseRow").data("request-url");
        
        $.get("/Expense/LoadExpenseTransactions/" + id, function (data) {
            $("#expenseTransactions").html(data);
        });
    };

    if (expId > 0) {
        
        loadTransactions(expId);
    }

    $('.expenseRow').on('click', function () {
        
        var self = $(this);
        var url = self.data('request-url');
        var expenseId = self.attr("id");
        $("#SelectedExpenseId").val(expenseId);

        $.get(url, function (data) {
            $("#expenseTransactions").html(data);
        });
    });

    $('#addExpenseButton').on('click', function () {
        var self = $(this);
        var url = self.data('request-url');
        $('.index').hide();
        $('#expenseTransactions').hide();

        $.get(url, function (data) {
            $("#editExpense").html(data);
        });
    });

    $('.editExpenseButton').on('click', function () {
        var self = $(this);
        var url = self.data('request-url');
        $('.index').hide();
        $('#expenseTransactions').hide();

        $.get(url, function (data) {
            $("#editExpense").html(data);
        });
    });
});