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
                <td>${item.description}</td>
                <td>$${item.amount.toLocaleString("de-DE")}</td>
                <td>${createDate(item.createdAt)}</td>
                <td class='d-flex flex-row justify-content-center'>
                    <button type='button' class='btn btn-outline-info btn-rounded btn-sm mr-2' onclick='edit(${JSON.stringify(item)})' data-toggle="modal" data-target="#modalCreate"><i class="bi bi-pencil"></i></button>
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
        title: "¿Seguro deseas eliminar este gasto?",
        text: "Esta acción no se puede deshacer",
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

function editExpense(entity) {
    $("#formContainer form input:not([type='hidden']").val("");
    $("input[name='CreateViewModel.ID']").val(entity.id);
    $("input[name='CreateViewModel.ID']").prop("disabled", false);

    $("#modalTitle").text("Editar gasto");
    $("#formContainer form").attr("action", "/Expenses/Edit");
    $("#formContainer form").attr("id", "form-edit");
    $("#btnSendModal").text("Confirmar");

    $("input[name='CreateViewModel.Description']").val(entity.description);
    $("input[name='CreateViewModel.Amount']").val(entity.amount);
    $("select[name='CreateViewModel.UserID']").val(entity.userID);
}

$(document).ready(function () {
    $("#btnSendModal").on("click", function () {
        if ($("#formContainer form").attr('id') === 'form-create') {
            sendForm("create");
        } else if ($("#formContainer form").attr('id') === 'form-edit') {
            sendForm("edit");
        }
    });
    $("#btnAdd").on("click", function () {
        $("#modalTitle").text("Agregar gasto");
        $("#formContainer form").attr("action", "/Expenses/Create");
        $("#formContainer form").attr("id", "form-create");
        $("#formContainer form input:not([type='hidden']").val("");
        $("#formContainer form select").val("");
        $("input[name='CreateViewModel.ID']").prop("disabled", true);
        $("#btnSendModal").text("Agregar");
    });

    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ gastos",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 gastos",
            "sInfoFiltered": "(filtrado de _MAX_ gastos en total)",
            "emptyTable": 'No hay gastos que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ gastos",
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