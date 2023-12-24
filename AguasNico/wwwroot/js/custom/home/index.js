
$(document).ready(function () {
    $('#DatePicker').bootstrapMaterialDatePicker({
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
        $('#routesTable tbody').html('');
        $('#form-searchRoutesByDate input[name="dateString"]').val(selectedDate);
        let form = $('#form-searchRoutesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) { //id, dealer, totalCarts, completedCarts, state, collected
                const row = response.routes.map(route =>`
                    <tr class="clickable" data-url="/route/details">
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        ${route.state == 'Pendiente' ? `
                        <td><span class="label label-warning">${route.state}</span></td>`
                        : `
                        <td><span class="label label-success">${route.state}</span></td>`}
                        <td>$ ${route.collected}</td>
                    </tr>
                `).join('');
                $('#routesTable tbody').append(row);
            },
            error: function (error) {
                // Maneja los errores aquí
                console.log('Error:', error);
            }
        });
    }
});

