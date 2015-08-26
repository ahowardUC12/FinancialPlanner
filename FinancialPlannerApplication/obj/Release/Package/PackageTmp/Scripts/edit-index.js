$(document).ready(function () {

    $('.expenseRow').on('click', function () {
        var self = $(this);
        var url = self.data('request-url');

        $.get(url, function (data) {
            $("#expenseTransactions").html(data);
        });
    });

    $('#addExpenseButton').on('click', function () {
        var self = $(this);
        var url = self.data('request-url');
        $('.index').hide();

        $.get(url, function (data) {
            $("#addExpense").html(data);
        });
    });

    $('.editExpenseButton').on('click', function () {
        var self = $(this);
        var url = self.data('request-url');
        $('.index').hide();

        $.get(url, function (data) {
            $("#editExpense").html(data);
        });
    });
});