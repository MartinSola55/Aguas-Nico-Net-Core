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
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.totalCarts}</td>
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