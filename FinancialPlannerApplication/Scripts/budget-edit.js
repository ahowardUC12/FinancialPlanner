$(document).ready(function() {

    $(".form-control").on("change", function() {
        $("#saveBudgetButton").prop("disabled", false);
    });

    $(".budgetName").on("keyup", function() {
        $("#saveBudgetButton").prop("disabled", false);
    });

    $("#editBudget #editBudgetForm #saveBudgetButton").on("click", function () {
        
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function(response) {
                $("#editBudget").html(response);
                $("#saveBudgetButton").prop("disabled", true);

            }
        });
    });

    $("#addBudget #editBudgetForm #saveBudgetButton").on("click", function () {
        
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addBudget").html(response);
                $("#saveBudgetButton").prop("disabled", true);
            }
        });
    });

    $("#editBudget #editBudgetForm #doneBudgetButton").on("click", function () {
        
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);

            }
        });
    });

    $("#addBudget #editBudgetForm #doneBudgetButton").on("click", function () {
        
        var form = $("#editBudgetForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
            }
        });
    });
});