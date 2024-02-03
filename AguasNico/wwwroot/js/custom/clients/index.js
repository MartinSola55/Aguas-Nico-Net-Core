$(document).ready(function () {
    $('#DataTable').DataTable({
        "order": false,
        "paging": false,
        "info": false,
        "searching": false,
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

    let timeoutId;
    $('#clientName').on('input', function () {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(searchByName, 1000);
    });
});

function searchByName() {
    const name = $('#clientName').val();

    if (name.length < 4)
        return toastrWarning("Debes ingresar al menos 4 caracteres para buscar por nombre");

    loadingRow();
    const form = $('#form-searchByName');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            fillTable(response.data)
        },
        error: function () {
            errorRow();
        }
    });
}

function loadingRow() {
    $('#DataTable tbody').empty();
    let loadingRow = `
    <tr>
        <td>
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only">Cargando...</span>
            </div>
        </td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>`;
    $('#DataTable tbody').append($(loadingRow));
}

function errorRow() {
    $('#DataTable tbody').empty();
    let row = `
    <tr>
        <td><h6 class="text-danger">No se pudo cargar la información</h6></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>`;
    $('#DataTable tbody').append($(row));
}

function fillTable(clients) {
    $('#DataTable tbody').empty();
    clients.forEach(item => {
        let row = `
            <tr data-id="${item.id}">
                <td><a href="/Clients/Details/${item.id}">${item.name}</a></td>
                <td>${item.address}</td>
                <td>${item.phone}</td>
                <td>$${item.debt}</td>
                <td>${item.dealer}</td>
            </tr>`;
        $('#DataTable tbody').append($(row));
    });
}