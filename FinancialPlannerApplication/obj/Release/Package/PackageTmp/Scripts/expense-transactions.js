$(document).ready(function () {
    $(".transactionRow").on("click", function () {
        var self = $(this);
        var transactionId = self.attr("id");
        var selectedExpenseId = $("#SelectedExpenseId").val();
        var url = self.data("request-url");

        $("#editExpense").hide();
        $("#indexExpense").hide();
        $('#addExpenseTransaction').hide();
        $('#expenseTransactions').hide();
        $("#addExpenseTransaction").hide();

        $.get(url + '/' + transactionId + '?expenseId=' + selectedExpenseId, function (data) {
            $('#editExpenseTransaction').html(data);
            $('#editExpenseTransaction').fadeIn("slow");
        });
    });

    $("#addExpenseTransactionButton").on("click", function () {
        
        var self = $(this);
        var url = self.data("request-url");
        var selectedAccountId = $("#SelectedExpenseId").val();
   
        $("#addExpenseTransaction").hide();
        $("#editExpense").hide();
        $("#indexExpense").hide();
        $("#expenseTransactions").hide();
        $('#editExpenseTransaction').hide();

        $.get(url + "/" + selectedAccountId, function (data) {
            $("#addExpenseTransaction").html(data);
            $("#addExpenseTransaction").fadeIn("slow");
        });
    });
});