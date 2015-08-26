$(document).ready(function () {

    var bgtId = $("#SelectedBudgetId").val();

    var getBudgetItems = function(id) {
        var url = $(".budgetRow").data("request-url");

        $.get(url + "/" + id, function (data) {
            $("#budgetDetails").html(data);
        });
    };

    if (bgtId > 0) {
        getBudgetItems(bgtId);
    }

    $("#addBudgetButton").on("click", function() {
        var self = $(this);
        var url = self.data("request-url");

        $("#addBudget").hide();
        $("#editBudget").hide();
        $("#budgetIndex").hide();
        $("#budgetDetails").hide();

        $.get(url, function(data) {
            $("#addBudget").html(data);
            $("#addBudget").fadeIn();
        });
    });

    $(".budgetRow").on("click", function () {
        var self = $(this);
        var budgetId = self.attr("id");
        var url = self.data("request-url");

        $.get(url + "/" + budgetId, function (data) {
            $("#budgetDetails").html(data);
        });
    });

    $(".editBudget").on("click", function () {
        var self = $(this);
        var budgetId = self.closest("tr").attr("id");
        var url = self.data("request-url");

        $("#budgetDetails").hide();
        $("#editBudget").hide();
        $("#addBudget").hide();
        $("#budgetIndex").hide();

        $.get(url + "/" + budgetId, function (data) {
            $("#editBudget").html(data);
            $("#editBudget").fadeIn();
        });
    });

   
});

