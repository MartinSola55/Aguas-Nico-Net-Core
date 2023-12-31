
$(document).ready(function () {
    $('#routesDay').on('change', function () {
        routesDay($(this).val());
    });
    $('#routesTable tbody tr').on('click', function () {
        window.location.href = $(this).data('url');
    });

    function routesDay(selectedDate) {
        $('#routesTable tbody').empty();
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
        $('#routesTable tbody').append($(loadingRow));
        $('#form-searchRoutesByDate input[name="dayString"]').val(selectedDate);
        let form = $('#form-searchRoutesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#routesTable tbody').empty();
                response.routes.forEach(route => {
                    let row = `<tr class="clickable" data-url="/Routes/Details/${route.id}">
                        <td>${route.dealer}</td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        ${route.state == 'Pendiente' ? `<td><span class="label label-warning">${route.state}</span></td>` : `<td><span class="label label-success">${route.state}</span></td>`}
                        <td>$${route.totalCollected}</td>
                        <td>${route.date}</td>
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
                    <td><h6 class="text-danger">No se pudo cargar la información</h6></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;
                $('#routesTable tbody').append($(row));
            }
        });
    }
});

