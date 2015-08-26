$(document).ready(function () {
    $('.budgetItemRow').on('click', function () {
        var self = $(this);
        var budgetItemId = self.attr('id');
        var url = self.data('request-url');

        $.get(url + '/' + budgetItemId, function (data) {
            $('#budgetItemTransactions').html(data);
        });
    });
});