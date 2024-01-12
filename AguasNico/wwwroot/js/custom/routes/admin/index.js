$(document).ready(function () {

    $('#daySelect').on('change', function () {
        routesDay($(this).val());
    });

    $('#routesTable tbody tr').on('click', function () {
        window.location.href = $(this).data('url');
    });

    function routesDay(daySelected) {
        $('#routesTable tbody').empty();
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
        </tr>`;
        $('#routesTable tbody').append($(loadingRow));
        $('#form-searchRoutesByDay input[name="dayString"]').val(daySelected);
        let form = $('#form-searchRoutesByDay');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#routesTable tbody').empty();
                response.routes.forEach(route => {
                    let row = `<tr class="clickable" data-url="/Routes/Details/${route.id}">
                        <td><span class="label label-info"><h6 class="text-white">${route.dealer}</h6></span></td>
                        <td><h6>${route.totalCarts}</h6></td>
                    </tr>`;
                    $('#routesTable tbody').append($(row));
                });
                $('#routesTable tbody tr').on('click', function () {
                    window.location.href = $(this).data('url');
                });
            },
            error: function (error) {
                $('#routesTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                </tr>`;
                $('#routesTable tbody').append($(row));
            }
        });
    }    
});

function renewAll() {
    Swal.fire({
        title: "Esta acción no se puede revertir",
        html: '¿Seguro deseas renovar <b>TODOS</b> los abonos? Esto incluye los clientes de todo el sistema.</br>Si un abono ya se renovó, no se volverá a renovar',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            let form = $("#form-renewAll");
            $.ajax({
                url: $(form).attr('action'),
                method: $(form).attr('method'),
                data: $(form).serialize(),
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message,
                        confirmButtonColor: '#1e88e5',
                        allowOutsideClick: true,
                    });
                },
                error: function (errorThrown) {
                    Swal.fire({
                        icon: 'error',
                        title: errorThrown.responseJSON.title,
                        html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error,
                        confirmButtonColor: '#1e88e5',
                    });
                }
            });
        }
    })
}