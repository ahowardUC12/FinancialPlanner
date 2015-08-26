$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveExpenseButton").prop("disabled", false);
    });

    $("#addExpense #editExpenseForm #saveExpenseButton").on("click", function () {
        
        var form = $("#editExpenseForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addExpense").html(response);
                $("#saveExpenseButton").prop("disabled", true);

            }
        });
    });

    $("#editExpense #editExpenseForm #saveExpenseButton").on("click", function () {
        
        var form = $("#editExpenseForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editExpense").html(response);
                $("#saveExpenseButton").prop("disabled", true);
            }
        });
    });

    $("#addExpense #editExpenseForm #doneExpenseButton").on("click", function () {
        
        var form = $("#editExpenseForm");

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

    $("#editExpense #editExpenseForm #doneExpenseButton").on("click", function () {
        
        var form = $("#editExpenseForm");

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