
$(document).ready(function () {
    let selectedDate = $('#DatePicker').val();
    salesDay(selectedDate);
    $('#DatePicker').bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });

    $('#DatePicker').on('change', function () {
        var selectedDate = $(this).val();
        salesDay(selectedDate);
    });

    function salesDay(date) {
        let form = $('#form-searchRoutesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) { //id, dealer, totalCarts, completedCarts, state, collected
                const cardContents = response.routes.map(route =>`
                    <tr class="clickable" data-url="/route/details">
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        ${route.state == 'Pendiente' ? `
                        <td><span class="label label-warning">${route.state}</span></td>`
                        : `
                        <td><span class="label label-success">${route.state}</span></td>`}
                        <td>${route.collected}</td>
                    </tr>
                `).join('');
                $('#salesResponse').html(cardInfo);
            },
            error: function (error) {
                // Maneja los errores aquí
                console.log('Error:', error);
            }
        });
    }
});

