$(document).ready(function () {

    var acctId = $("#SelectedAccountId").val();

    var loadTransactions = function (id) {
        var url = $(".accountRow").data("request-url");

        $.get(url + "/" + id, function (data) {
            $("#accountTransactions").html(data);
        });
    };

    if (acctId > 0) {
        //$(".accountRow").click(acctId);
        loadTransactions(acctId);
    }

    var hideDivs = function() {
        $("#fpaIndex").hide();
        $("#accountTransactions").hide();
        $("#editAccount").hide();
    };

    $(".editAccount").on("click", function () {
        var self = $(this);
        var accountId = self.attr("id");
        var url = self.data("request-url");

        hideDivs();
        //$("#fpaIndex").hide();
        ////$("#accountsTable").hide();
        ////$("#addAccountButton").hide();
        //$("#accountTransactions").hide();
        //$("#editAccount").hide();

        $.get(url + "/" + accountId, function (data) {         
            $("#editAccount").html(data);
            $("#editAccount").fadeIn("slow");
        });
    });

    $("#addAccountButton").on("click", function () {
        var self = $(this);
        var url = self.data("request-url");

        hideDivs();
        //$("#fpaIndex").hide();
        ////$("#addAccountButton").hide();
        ////$("#accountTransactions").hide();
        //$("#accountsTable").hide();
        //$("#addAccount").hide();

        $.get(url, function (data) {
            $("#addAccount").html(data);
            $("#addAccount").fadeIn("slow");
        });
    });

    $(".accountRow").on("click", function () {
        var self = $(this);
        var accountId = self.attr("id");
        var url = self.data("request-url");
        $("#SelectedAccountId").val(accountId);

        $.get(url + "/" + accountId, function (data) {
            $("#accountTransactions").html(data);
        });
    });
});