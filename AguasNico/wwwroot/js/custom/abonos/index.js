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
                
                if (action === 'delete') {
                    Swal.fire({
                        icon: 'success',
                        title: response.message,
                        confirmButtonColor: '#1e88e5',
                    });
                    removeFromTable(response.data);
                    return;
                }

                Swal.fire({
                    title: response.message,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#1e88e5',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
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

    const dayOfWeek = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
    $("#selectAbono").on('change', function () {
        $("#clientsTable tbody").empty();
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
        </tr>`;
        $('#clientsTable tbody').append($(loadingRow));
        let form = $('#form-searchClients');
        $("#form-searchClients input[name='abonoID']").val($(this).val());
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#clientsTable tbody').empty();
                response.data.forEach(client => {
                    let row = `<tr>
                        <td>${client.name}</td>
                        <td>${client.address}</td>
                        <td>${client.dealer != null ? client.dealer.name : "Sin repartidor"} - ${client.deliveryDay != null ? dayOfWeek[client.deliveryDay] : "Sin día de reparto"}</td>
                    </tr>`;
                    $('#clientsTable tbody').append($(row));
                });
            },
            error: function (error) {
                $('#clientsTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                </tr>`;
                $('#clientsTable tbody').append($(row));
            }
        });
    });
});
