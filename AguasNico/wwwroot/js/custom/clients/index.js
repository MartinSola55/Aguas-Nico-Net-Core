$(document).ready(function () {
    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ clientes",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 clientes",
            "sInfoFiltered": "(filtrado de _MAX_ clientes en total)",
            "emptyTable": 'No hay clientes que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ clientes",
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