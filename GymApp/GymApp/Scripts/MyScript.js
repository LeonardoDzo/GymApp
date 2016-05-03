$(function () {
   
    $(".datepicker").datepicker({
        dateFormat: "yy-mm-dd",
        changeYear: true,
        showAnim: "slideDown",
        showWeek: true
    });
    $(".numeric").numeric();
});

$('#Enviar').click(function () {
    var fecha1 = $('#fInicio').val();
    var fecha2 = $('#ffin').val();

    if (fecha2 <= fecha1) {
        alert("Por favor ponga una fecha mayor a la fecha de inicio");
        $('#ffin').val("");
    }
});