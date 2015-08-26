$(document).ready(function () {
    $(".budgetItemRow").on("click", function () {
        var self = $(this);
        var budgetItemId = self.attr("id");
        var url = self.data("request-url");

        $.get(url + "/" + budgetItemId, function (data) {
            $("#budgetItemTransactions").html(data);
        });
    });

    var bgtItemId = $("#SelectedBudgetItemId").val();

    var getBudgetItemTransactions = function (id) {
        
        $.get("/Budget/LoadBudgetItemTransactions/" + id, function (data) {
            $("#budgetItemTransactions").html(data);
        });
    };

    if (bgtItemId > 0) {
        getBudgetItemTransactions(bgtItemId);
    }


    $(".editBudgetItem").on("click", function () {
        var self = $(this);
        var budgetItemId = self.closest("tr").attr("id");
        var url = self.data("request-url");

        $("#budgetItemTransactions").hide();
        $("#budgetItemsView").hide();
        $("#budgetIndex").hide();
        //$("#budgetItems").hide();
        //$('#budgetsTable').hide();
        $("#editBudgetItem").hide();
        $("#addBudget").hide();

        $.get(url + "/" + budgetItemId, function (data) {
            $("#editBudgetItem").html(data);
            $("#editBudgetItem").fadeIn("slow");
        });
    });

    $("#addBudgetItemButton").on("click", function () {
        var self = $(this);
        var url = self.data("request-url");

        $("#budgetItemTransactions").hide();
        $("#budgetItems").hide();
        $("#budgetItemsView").hide();
        $("#budgetIndex").hide();
        //$('#budgetsTable').hide();
        $("#editBudgetItem").hide();
        $("#addBudget").hide();
        $("#addBudgetItem").hide();
        $("#addBudgetItemButton").hide();

        $.get(url, function (data) {
            $("#addBudgetItem").html(data);
            $("#addBudgetItem").fadeIn("slow");
        });
    });
});