
$(document).ready(function () {
    $('#routesTable').DataTable({
        "order": false,
        "info": false,
        "paging": false,
        "searching": false,
        "language": {
            "emptyTable": "No hay planillas para mostrar",
            "zeroRecords": "No hay planillas para mostrar"
        },
    });
    $('#DatePicker, #DatePickerProducts, #DatePickerExpenses').bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });

    salesDay($('#DatePicker').val());

    $('#DatePicker').on('change', function () {
        salesDay($(this).val());
    });

    function salesDay(selectedDate) {
        $('#routesTable').DataTable().clear().draw();
        let loadingRow = `<tr>
            <td>
                <div class="spinner-border text-primary" role="status">
                    <span class="sr-only">Cargando...</span>
                </div>
            </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>`;
        $('#routesTable').DataTable().rows.add($(loadingRow)).draw();
        $('#form-searchRoutesByDate input[name="dateString"]').val(selectedDate);
        let form = $('#form-searchRoutesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#routesTable').DataTable().clear().draw();
                response.routes.forEach(route => {
                    let row = `<tr class="clickable" data-url="/route/details">
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        ${route.state == 'Pendiente' ? `<td><span class="label label-warning">${route.state}</span></td>` : `<td><span class="label label-success">${route.state}</span></td>`}
                        <td>$ ${route.collected}</td>
                    </tr>`;
                    $('#routesTable').DataTable().rows.add($(row)).draw();
                });
            },
            error: function (error) {
                $('#routesTable').DataTable().clear().draw();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;
                $('#routesTable').DataTable().rows.add($(row)).draw();
            }
        });
    }
});

