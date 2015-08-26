$(document).ready(function() {
    
    $(".editBudgetItemTransaction").on("click", function () {
        var self = $(this);
        var url = self.data("request-url");
        var transactionId = self.attr("id");

        $("#addBudget").hide();
        $("#budgetsTable").hide();
        $("#budgetItems").hide();
        $("#editBudgetItemTransaction").hide();
        $("#budgetItemTransactionsSection").hide();
        $("#budgetItemsView").hide();
        $("#budgetIndex").hide();

        $.get(url + "/" + transactionId, function (data) {
            $("#editBudgetItemTransaction").html(data);
            $("#editBudgetItemTransaction").fadeIn("slow");
        });
    });

    $("#addBudgetItemTransactionButton").on("click", function () {
        var self = $(this);
        var url = self.data("request-url");
        var budgetItemId = $("#BudgetItemId").val();
       
        $("#addBudget").hide();
        $("#budgetsTable").hide();
        $("#budgetItems").hide();
        $("#editBudgetItemTransaction").hide();
        $("#budgetItemTransactionsSection").hide();
        $("#budgetItemsView").hide();
        $("#budgetIndex").hide();

         $.get(url + "/" + budgetItemId, function (data) {
            $("#addBudgetItemTransaction").html(data);
            $("#addBudgetItemTransaction").fadeIn("slow");
        });
    });
});