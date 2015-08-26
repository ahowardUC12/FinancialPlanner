$(document).ready(function () {

    $(".form-control").on("change", function () {
        $("#saveBudgetItemButton").prop("disabled", false);
    });

    $(".budgetItemFormItem").on("keyup", function() {
        $("#saveBudgetItemButton").prop("disabled", false);
    });

    $("#editBudgetItem #editBudgetItemForm #saveBudgetItemButton").on("click", function () {
        
        var form = $("#editBudgetItemForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=false",
            //url: "/Budget/EditBudgetItem",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editBudgetItem").html(response);
                $("#saveBudgetItemButton").prop("disabled", true);

            }
        });
    });

    $("#addBudgetItem #editBudgetItemForm #saveBudgetItemButton").on("click", function () {
        
        var form = $("#editBudgetItemForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=false",
            //url: "/Budget/AddBudgetItem",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addBudgetItem").html(response);
                $("#saveBudgetItemButton").prop("disabled", true);
            }
        });
    });

    $("#editBudgetItem #editBudgetItemForm #doneEditBudgetItem").on("click", function () {
        
        var form = $("#editBudgetItemForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            //url: "/Budget/EditBudgetItem",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
               // $("#saveBudgetItemButton").prop("disabled", true);

            }
        });
    });

    $("#addBudgetItem #editBudgetItemForm #doneEditBudgetItem").on("click", function () {
        
        var form = $("#editBudgetItemForm");

        $.ajax(
        {
            url: form.attr("action") + "?isDone=true",
            //url: "/Budget/AddBudgetItem",
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("body").html(response);
                //$("#saveBudgetItemButton").prop("disabled", true);
            }
        });
    });

});