$(document).ready(function() {
   // $(".datepicker").datepicker({ autoclose: true });

    $('.datepicker').datepicker({
        format: 'mm/dd/yyyy',
        startDate: '-3d'
    });
});