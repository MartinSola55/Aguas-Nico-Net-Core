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
            "emptyTable": 'No hay clientes que coincidan con la b�squeda',
            "sLengthMenu": "Mostrar _MENU_ clientes",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "�ltimo",
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
        },
    });

    let timeoutName;
    $('#clientName').on('input', function () {
        clearTimeout(timeoutName);
        timeoutName = setTimeout(searchByName, 1000);
    });

    let timeoutId;
    $('#clientID').on('input', function () {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(searchByID, 1000);
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
            if (response.data.length === 0) {
                emptyTable();
                return;
            }
            fillTable(response.data)
        },
        error: function () {
            errorRow();
        }
    });
}

function searchByID() {
    loadingRow();
    const form = $('#form-searchByID');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            if (response.data.length === 0) {
                emptyTable();
                return;
            }
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
        <td><h6 class="text-danger">No se pudo cargar la informaci�n o el cliente no existe</h6></td>
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
            <tr data-id="${item.id}" class="clickable" onclick="openDialog(${item.id})">
                <td>${item.name}</td>
                <td>${item.address}</td>
                <td>${item.phone}</td>
                <td>$${item.debt}</td>
                <td>${item.dealer}</td>
            </tr>`;
        $('#DataTable tbody').append($(row));
    });
}

function emptyTable() {
    $('#DataTable tbody').empty();
    let row = `
    <tr>
        <td colspan="5"><h6>No hay clientes que coincidan con la b�squeda</h6></td>
    </tr>`;
    $('#DataTable tbody').append($(row));
}

function openDialog(id) {
    Swal.fire({
        title: 'Ingresa el total de la transferencia',
        input: 'number',
        inputAttributes: {
            min: 1,
            step: 1,
        },
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        },
        inputValidator: (value) => {
            if (!value || value <= 0) {
                return 'Por favor, ingrese un monto v�lido';
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-create input[name='Transfer.Amount']").val($("#swal2-input").val());
            $("#form-create input[name='Transfer.ClientID']").val(id);
            askForDate();
        }
    });
}

function askForDate() {
    Swal.fire({
        icon: 'question',
        title: '�Deseas modificar la fecha de la transferencia?',
        showCancelButton: true,
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            changeDate();
        } else {
            sendForm("create");
        }
    });
}

function changeDate() {
    Swal.fire({
        title: 'Ingresa la fecha de la transferencia',
        input: 'date',
        inputAttributes: {
            max: new Date().toISOString().split("T")[0],
        },
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        },
        inputValidator: (value) => {
            if (!value) {
                return 'Por favor, ingrese una fecha v�lida';
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-create input[name='Transfer.Date']").val($("#swal2-input").val());
            sendForm("create");
        }
    })
}

function sendForm(action) {
    let form = document.getElementById(`form-${action}`);

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
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
                    window.location.href = window.location.origin + "/Transfers";
                }
            });
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
}