$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveAccountTransactionButton").prop("disabled", false);
    });

    $(".form-control").on("change", function () {
        $("#saveAccountTransactionButton").prop("disabled", false);
    });

    $("#addAccountTransaction #editTransactionForm #saveAccountTransactionButton").on("click", function () {
        var form = $("#editTransactionForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addAccountTransaction").html(response);
                $("#saveAccountTransactionButton").prop("disabled", true);

            }
        });
    });

    $("#editTransaction #editTransactionForm #saveAccountTransactionButton").on("click", function () {
        var form = $("#editTransactionForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editTransaction").html(response);
                $("#saveAccountTransactionButton").prop("disabled", true);
            }
        });
    });

    $("#addAccountTransaction #editTransactionForm #doneEditAccountTransactionButton").on("click", function () {
        var form = $("#editTransactionForm");

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

    $("#editTransaction #editTransactionForm #doneEditAccountTransactionButton").on("click", function () {
        var form = $("#editTransactionForm");

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
        var form = $("#editTransactionForm");

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
                var isEdit = $("#editTransaction").is(":visible");
                if (isEdit) {
                    $("#editTransaction").html(response);

                    var isAddVisible = $("#addAccountTransaction").is(":visible");
                    if (isAddVisible) {
                        $("#addAccountTransaction").hide();
                    }
                } else {
                    $("#addAccountTransaction").html(response);

                    var isEditVisible = $("#editTransaction").is(":visible");
                    if (isEditVisible) {
                        $("#editTransaction").hide();
                    }
                }

            }
        });
    });

});