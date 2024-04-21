function createDate(dateString) {
    let date = new Date(dateString);
    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return day + "/" + month + "/" + year;
}

function fillTable(item) {
    let content = `
        <tr data-id='${item.id}'>
            <td>${item.client}</td>
            <td>${item.dealer}</td>
            <td>$${item.amount.toLocaleString("de-DE")}</td>
            <td>${createDate(item.date)}</td>
            <td>${createDate(item.createdAt)}</td>
            <td class='d-flex flex-row justify-content-center'>
                <button type='button' class='btn btn-outline-info btn-rounded btn-sm mr-2' onclick='editTransfer(${JSON.stringify(item)})' data-toggle="modal" data-target="#modalCreate"><i class="bi bi-pencil"></i></button>
                <button type='button' class='btn btn-danger btn-rounded btn-sm ml-2' onclick='deleteObj(${item.id})'><i class='bi bi-trash3'></i></button>
            </td>
        </tr>`;
    $('#DataTable').DataTable().row.add($(content)).draw();
}

function removeFromTable(id) {
    $('#DataTable').DataTable().row(`[data-id="${id}"]`).remove().draw();
}

function deleteObj(id) {
    Swal.fire({
        title: "¿Seguro deseas eliminar esta transferencia?",
        html: "Esta acción no se puede deshacer.<br>Además, se restablecerá el saldo cargado al cliente",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Eliminar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-delete input[name='id']").val(id);
            sendForm("delete");
        }
    });
}

function sendForm(action) {
    let form = document.getElementById(`form-${action}`);

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#btnCloseModalCreate").click();
            Swal.fire({
                icon: 'success',
                title: response.message,
                confirmButtonColor: '#1e88e5',
            });
            if (action === 'create') {
                fillTable(response.data);
            } else if (action === 'edit') {
                removeFromTable(response.data.id);
                fillTable(response.data);
            } else {
                removeFromTable(response.data);
            }
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

function editTransfer(entity) {
    Swal.fire({
        title: 'Ingresa el nuevo monto de la transferencia',
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
                return 'Por favor, ingrese un monto válido';
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-edit input[name='Transfer.Amount']").val($("#swal2-input").val());
            $("#form-edit input[name='Transfer.ID']").val(entity.id);
            askForDate();
        }
    });
}

function askForDate() {
    Swal.fire({
        icon: 'question',
        title: '¿Deseas modificar la fecha de la transferencia?',
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
            $("#form-edit input[name='updateDate']").val(false);
            sendForm("edit");
        }
    });
}

function changeDate() {
    Swal.fire({
        title: 'Nueva fecha',
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
                return 'Por favor, ingrese una fecha válida';
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-edit input[name='Transfer.Date']").val($("#swal2-input").val());
            $("#form-edit input[name='updateDate']").val(true);
            sendForm("edit");
        }
    })
}

function getTransfers() {
    const date = $('#DatePicker').val();
    $('#table_body').empty();
    const loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>`;
    $('#table_body').append($(loadingRow));
    $('#form-search input[name="dateString"]').val(date);
    const form = $('#form-search');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $('#table_body').empty();
            response.data.forEach(transfer => {
                const row = `
                    <tr data-id="${transfer.id}">
                        <td>${transfer.client}</td>
                        <td>${transfer.dealer}</td>
                        <td>$${transfer.amount}</td>
                        <td>${transfer.date.toLocaleString()}</td>
                        <td>${transfer.createdAt.toLocaleString()}</td>
                        <td class='d-flex flex-row justify-content-center'>
                            <button type='button' class='btn btn-outline-info btn-rounded btn-sm mr-2' onclick='editTransfer(${JSON.stringify(transfer)})' data-toggle="modal" data-target="#modalCreate"><i class="bi bi-pencil"></i></button>
                            <button type='button' class='btn btn-danger btn-rounded btn-sm ml-2' onclick='deleteObj(${transfer.id}'><i class='bi bi-trash3'></i></button>
                        </td>
                    </tr>`;
                $('#table_body').append($(row));
            });
        },
        error: function (error) {
            $('#table_body').empty();
            const row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;
            $('#table_body').append($(row));
        }
    });
}

$(document).ready(function () {
    $("#DatePicker").bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });   

    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ transferencias",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 transferencias",
            "sInfoFiltered": "(filtrado de _MAX_ transferencias en total)",
            "emptyTable": 'No hay transferencias que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ transferencias",
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