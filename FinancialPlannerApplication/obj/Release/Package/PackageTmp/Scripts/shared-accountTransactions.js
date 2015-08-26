$(document).ready(function () {
    $(".transactionRow").on("click", function () {
        var self = $(this);
        var transactionId = self.attr("id");
        var selectedAccountId = $("#SelectedAccountId").val();
        var url = self.data("request-url");

        $("#addAccount").hide();
        $("#editAccount").hide();
        $("#accountTransactionsView").hide();
        $("#fpaIndex").hide();
        $("#editTransaction").hide();
        $("#addAccountTransaction").hide();

        $.get(url + "/" + transactionId + "?accountId=" + selectedAccountId , function (data) {
            $("#editTransaction").html(data);
            $("#editTransaction").fadeIn("slow");
        });
    });

    $("#addAccountTransactionButton").on("click", function () {
        var self = $(this);
        var url = self.data("request-url");
        var selectedAccountId = $("#SelectedAccountId").val();
        $("#addAccount").hide();
        $("#editAccount").hide();
        $("#accountTransactionsView").hide();
        $("#fpaIndex").hide();
        $("#editTransaction").hide();
        $("#addAccountTransaction").hide();

        $.get(url + "/" + selectedAccountId, function (data) {
            $("#addAccountTransaction").html(data);
            $("#addAccountTransaction").fadeIn("slow");
        });
    });
});