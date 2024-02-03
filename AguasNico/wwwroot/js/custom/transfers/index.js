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

    let errors = $(".input-validation-error");

    if (errors.length === 0) {
        // Enviar solicitud AJAX
        $.ajax({
            url: $(form).attr('action'), // Utiliza la ruta del formulario
            method: $(form).attr('method'), // Utiliza el método del formulario
            data: $(form).serialize(), // Utiliza los datos del formulario
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
}

function editTransfer(entity) {
    $("#formContainer form input:not([type='hidden']").val("");
    $("input[name='CreateViewModel.ID']").val(entity.id);
    $("input[name='CreateViewModel.ID']").prop("disabled", false);

    $("#modalTitle").text("Editar transferencia");
    $("#formContainer form").attr("action", "/Transfers/Edit");
    $("#formContainer form").attr("id", "form-edit");
    $("#btnSendModal").text("Confirmar");

    $("input[name='CreateViewModel.Date']").val(createDate(entity.date));
    $("input[name='CreateViewModel.Amount']").val(entity.amount);
    $("input[name='CreateViewModel.ClientID']").val("");

    $('#contentTableClients').hide();
    $('#contentSearchClients').hide();
    $('#modalCreate .modal-footer').show();
}

$(document).ready(function () {
    $("input[name='CreateViewModel.Date']").bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });

    $("#btnSendModal").on("click", function () {
        if ($("#formContainer form").attr('id') === 'form-create') {
            sendForm("create");
        } else if ($("#formContainer form").attr('id') === 'form-edit') {
            sendForm("edit");
        }
    });
    $("#btnAdd").on("click", function () {
        $("#modalTitle").text("Agregar transferencia");
        $("#formContainer form").attr("action", "/Transfers/Create");
        $("#formContainer form").attr("id", "form-create");
        $("#formContainer form input:not([type='hidden']").val("");
        $("#formContainer form select").val("");
        $("input[name='CreateViewModel.ID']").prop("disabled", true);
        $("#btnSendModal").text("Agregar");

        $('#contentSearchClients').show();
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

    $('#btnSearchClients').click(function () {
        if ($("#searchClient").val().length < 3) {
            Swal.fire({
                icon: 'warning',
                title: 'Alerta',
                text: 'Debes ingresar al menos tres caracteres para buscar el cliente',
                confirmButtonColor: '#1e88e5',
            });
            return;
        }
        $('#tableClients tbody').empty();
        let loadingRow = `<tr>
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
        $('#tableClients tbody').append($(loadingRow));

        let name = $('#searchClient').val();
        $('#form-searchClients input[name="name"]').val(name);

        let form = $('#form-searchClients');
        $.ajax({
            url: $(form).attr('action'),
            type: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#tableClients tbody').empty();
                if (response.data.length == 0) {
                    let row = `
                    <tr>
                        <td colspan="5" class="text-center">No se encontraron resultados</td>
                    </tr>`;
                    $('#tableClients tbody').append($(row));
                    $('#contentTableClients').show();
                    return;
                }
                response.data.forEach(client => {
                    let row = `
                        <tr>
                            <td>${client.name}</td>
                            <td>${client.address}</td>
                            <td>${client.dealer}</td>
                            <td>$${client.debt}</td>
                            <td><button type='button' class='btn btn-info btn-rounded btn-sm' onclick='selectClient(${JSON.stringify(client)})'>Seleccionar</button></td>
                        </tr>`;
                    $('#tableClients tbody').append($(row));
                });
                $('#contentTableClients').show();
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
    });
});

function selectClient(client) {
    $('#form-create input[name="CreateViewModel.ClientID"]').val(client.id);
    $('#searchClient').val(client.name);
    $('#contentTableClients').hide();
    $('#modalCreate .modal-footer').show();
}