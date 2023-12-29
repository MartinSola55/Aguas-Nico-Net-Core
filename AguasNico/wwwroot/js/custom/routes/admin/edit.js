function removeFromTable(client_id, table_id) {
    $(`#${table_id}`).DataTable().row(`[data-id="${client_id}"]`).remove().draw();
}

function fillTable(client, table_id, action, btn_color, btn_icon, btn_size = "") {
    let totalClients = $(`#${table_id}`).DataTable().rows().count() + 1;
    let index = "";
    if (table_id == 'clientsInRouteTable') {
        index = `<td style='cursor: pointer'>${totalClients}</td>`;
    }
    let content = `
        <tr data-id='${client.id}'>
            ${index}
            <td class="text-center">
                <button type="button" class="btn btn-${btn_color} ${btn_size}" onclick='${action}(${JSON.stringify(client)})'><i class="bi bi-${btn_icon}"></i></button>
            </td>
            <td>${client.name}</td>
            <td>${client.address}</td>
        </tr>`;
    $(`#${table_id}`).DataTable().row.add($(content)).draw();
    if (table_id == 'clientsNotInRouteTable') {
        $(`#${table_id}`).DataTable().order([1, 'asc']).draw();
    }
}

function addClient(client) {
    removeFromTable(client.id, 'clientsNotInRouteTable');
    fillTable(client, 'clientsInRouteTable', 'removeClient', 'danger', 'x-lg', 'btn-sm');
}

function removeClient(client) {
    removeFromTable(client.id, 'clientsInRouteTable');
    fillTable(client, 'clientsNotInRouteTable', 'addClient', 'info', 'arrow-left');
}

function createClientsArray() {
    let clients = [];

    if ($("#clientsInRouteTable").DataTable().rows().count() == 0) {
        Swal.fire({
            icon: 'warning',
            title: 'No se ha seleccionado ningún cliente',
            confirmButtonColor: '#1e88e5',
        });
        return;
    }

    $("#clientsInRouteTable").DataTable().rows().every(function () {
        let removeButton = $(this.node()).find('button[name="remove_client"]');
        if (!removeButton.is(':disabled')) {
            clients.push({
                ID: parseInt($(this.node()).data('id')),
            });
        }
    });

    let clientsData = {
        Clients: clients
    };

    $.ajax({
        url: $("#form-confirm").attr('action'),
        method: $("#form-confirm").attr('method'),
        data: $("#form-confirm").serialize() + "&" + $.param(clientsData),
        success: function (response) {
            Swal.fire({
                title: response.message,
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#1e88e5',
                confirmButtonText: 'OK',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = "/Routes/Details/" + response.id;
                }
            })
        },
        error: function (errorThrown) {
            Swal.fire({
                icon: 'error',
                title: errorThrown.responseJSON.title,
                text: errorThrown.responseJSON.message,
                confirmButtonColor: '#1e88e5',
            });
        }
    });
};

$(document).ready(function () {
    $('#clientsNotInRouteTable').DataTable({
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ clientes",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 clientes",
            "sInfoFiltered": "(filtrado de _MAX_ clientes en total)",
            "emptyTable": 'No hay clientes que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ clientes",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
        },
    });
    $('#clientsInRouteTable').DataTable({
        rowReorder: {
            selector: 'td:first-child',
            update: true
        },
        columnDefs: [
            { orderable: false, targets: [0] }
        ],
        scrollY: '50vh',
        scrollCollapse: true,
        paging: false,
        info: false,
        searching: false,
    });
});