function fillTable(item) {
    let content = `
            <tr data-id='${item.id}'>
                <td>${item.name}</td>
                <td>$${item.price.toLocaleString("de-DE")}</td>
                <td class='d-flex flex-row justify-content-center'>
                    <button type='button' class='btn btn-outline-info btn-rounded btn-sm mr-2' onclick='edit(${JSON.stringify(item)})' data-toggle="modal" data-target="#modal"><i class="bi bi-pencil"></i></button>
                    <a class="btn btn-outline-info btn-rounded btn-sm" href="/Abonos/Edit/${item.id}"><i class="bi bi-box-seam"></i></a>
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
        title: "¿Seguro deseas eliminar este abono?",
        html: "Esta acción no se puede deshacer.<br/>Esto desasociará el abono a todos los clientes que lo posean.",
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

        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $("#btnCloseModal").click();
                Swal.fire({
                    icon: 'success',
                    title: response.message,
                    confirmButtonColor: '#1e88e5',
                });
                if (action === 'edit') {
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

function edit(entity) {
    $("#formContainer form input:not([type='hidden']").val("");
    $("input[name='EditedAbono.ID']").val(entity.id);
    $("input[name='EditedAbono.ID']").prop("disabled", false);

    $("#btnSendModal").text("Confirmar");

    $("input[name='EditedAbono.Name']").val(entity.name);
    $("input[name='EditedAbono.Price']").val(entity.price);
}

$(document).ready(function () {
    $('#DataTable').DataTable({
        "order": false,
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ abonos",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 abonos",
            "sInfoFiltered": "(filtrado de _MAX_ abonos en total)",
            "emptyTable": 'No hay abonos que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ abonos",
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