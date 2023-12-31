
$(document).ready(function () {
    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ repartidores",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 repartidores",
            "sInfoFiltered": "(filtrado de _MAX_ repartidores en total)",
            "emptyTable": 'No hay repartidores que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ repartidores",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
        },
    });
});