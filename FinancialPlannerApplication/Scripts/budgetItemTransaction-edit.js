$(document).ready(function() {
    $(".form-control").on("change", function () {
        $("#saveBudgetItemTransactionButton").prop("disabled", false);
    });

    $(".budgetItemFormItemTransaction").on("keyup", function () {
        $("#saveBudgetItemTransactionButton").prop("disabled", false);
    });

    $("#editBudgetItemTransaction #editBudgetTransactionForm #saveBudgetItemTransactionButton").on("click", function () {
        var form = $("#editBudgetTransactionForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=false",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editBudgetItemTransaction").html(response);
                $("#saveBudgetItemTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#addBudgetItemTransaction #editBudgetTransactionForm #saveBudgetItemTransactionButton").on("click", function () {
        var form = $("#editBudgetTransactionForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=false",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addBudgetItemTransaction").html(response);
                $("#saveBudgetItemTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#editBudgetItemTransaction #editBudgetTransactionForm #doneEditBudgetTransaction").on("click", function () {
        var form = $("#editBudgetTransactionForm");

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

    $("#addBudgetItemTransaction #editBudgetTransactionForm #doneEditBudgetTransaction").on("click", function () {
        
        var form = $("#editBudgetTransactionForm");

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

    $("#SelectedBudgetId").on("change", function () {
        
        var form = $("#editBudgetTransactionForm");

        var values = form.serializeArray();

        for (var index = 0; index < values.length; ++index) {
            if (values[index].name === "FindingBudgetItems") {
                values[index].value = true;
                break;
            }
        }

        $.ajax({
            url: form.attr("action"),
            type: "post",
            data: values,
            success: function (response) {
                var isEdit = $("#editBudgetItemTransaction").is(":visible");
                if (isEdit) {
                    $("#editBudgetItemTransaction").html(response);
                    $("#saveBudgetItemTransactionButton").prop("disabled", false);
                } else {
                    $("#addBudgetItemTransaction").html(response);
                    $("#saveBudgetItemTransactionButton").prop("disabled", false);
                }

            }
        });
    });
});