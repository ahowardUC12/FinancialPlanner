$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveExpenseTransactionButton").prop("disabled", false);
    });

    $(".form-control").on("change", function () {
        $("#saveExpenseTransactionButton").prop("disabled", false);
    });

    $("#addExpenseTransaction #editExpenseTransactionForm #saveExpenseTransactionButton").on("click", function () {
        
        var form = $("#editExpenseTransactionForm");
        var formUrl = form.attr("action");

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addExpenseTransaction").html(response);
                $("#saveExpenseTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#editExpenseTransaction #editExpenseTransactionForm #saveExpenseTransactionButton").on("click", function () {
        
        var form = $("#editExpenseTransactionForm");
        var formUrl = form.attr("action");
        //var splitUrl = formUrl.split("/");
        //var lastItemSplitUrl = splitUrl[splitUrl.length - 1];

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editExpenseTransaction").html(response);
                $("#saveExpenseTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#addExpenseTransaction #editExpenseTransactionForm #doneEditExpenseTransactionButton").on("click", function () {
        var form = $("#editExpenseTransactionForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
                //$("#saveAccountTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#editExpenseTransaction #editExpenseTransactionForm #doneEditExpenseTransactionButton").on("click", function () {
        var form = $("#editExpenseTransactionForm");
        var formUrl = form.attr("action");
        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
                //$("#saveAccountTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#SelectedBudgetId").on("change", function () {
        
        var form = $("#editExpenseTransactionForm");

        var values = form.serializeArray();

        for (var index = 0; index < values.length; ++index) {
            if (values[index].name === "FindingBudgetItems") {
                values[index].value = true;
                break;
            }
        }

        var isEdit = $("#editExpenseTransaction").is(":visible");

        //var url = "/Expense/AddTransaction";
        //if (isEdit)
        //    url = "/Expense/EditTransaction";

        $.ajax({
            //url: url,
            url: form.attr("action"),
            type: "post",
            data: values,
            success: function (response) {
                
                if (isEdit) {
                    $("#editExpenseTransaction").html(response);
                } else {
                    $("#addExpenseTransaction").html(response);
                }

            }
        });
    });

});