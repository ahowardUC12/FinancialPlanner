$(document).ready(function () {
    $(".form-control").on("keyup", function () {
        $("#saveAccountButton").prop("disabled", false);
    });

    $("#addAccount #editAccountForm #saveAccountButton").on("click", function () {
        
        var form = $("#editAccountForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#addAccount").html(response);
                $("#saveAccountButton").prop("disabled", true);

            }
        });
    });

    $("#editAccount #editAccountForm #saveAccountButton").on("click", function () {
        
        var form = $("#editAccountForm");

        $.ajax(
        {
            url: form.attr("action"),
            type: "post",
            data: form.serialize(),
            success: function (response) {
                $("#editAccount").html(response);
                $("#saveAccountButton").prop("disabled", true);
            }
        });
    });

    $("#addAccount #editAccountForm #doneAccountButton").on("click", function () {
        
        var form = $("#editAccountForm");

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

    $("#editAccount #editAccountForm #doneAccountButton").on("click", function () {
        
        var form = $("#editAccountForm");

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