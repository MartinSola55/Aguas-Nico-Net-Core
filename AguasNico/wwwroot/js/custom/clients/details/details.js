$(document).ready(function () {
    $('#TableProducts').DataTable({
        "order": false,
        "paging": false,
        "info": false,
        "searching": false,
    });

    $('#TableHistory').DataTable({
        "order": false,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollY": "50vh",
        "scrollCollapse": true,
        "language": {
            "emptyTable": "No hay datos disponibles",
        }
    });

    $('#TableProductsHistory').DataTable({
        "order": false,
        "paging": false,
        "info": false,
        "searching": false,
        "scrollY": "50vh",
        "scrollCollapse": true,
        "language": {
            "emptyTable": "No hay datos disponibles",
        }
    });
});