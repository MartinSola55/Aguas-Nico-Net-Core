
$(document).ready(function () {
    $('#ExpensesDatePicker, #ProductsDatePicker, #RoutesDatePicker').bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });

    routesDay($('#RoutesDatePicker').val());

    $('#RoutesDatePicker').on('change', function () {
        routesDay($(this).val());
    });
    $('#ProductsDatePicker').on('change', function () {
        productsDay($(this).val());
    });
    $('#ExpensesDatePicker').on('change', function () {
        expensesDay($(this).val());
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
        </tr>`;
        $('#routesTable tbody').append($(loadingRow));
        $('#form-searchRoutesByDate input[name="dateString"]').val(selectedDate);
        let form = $('#form-searchRoutesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#routesTable tbody').empty();
                response.routes.forEach(route => {
                    let row = `<tr class="clickable" data-url="/Routes/Details/${route.id}">
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        ${route.state == 'Pendiente' ? `<td><span class="label label-warning">${route.state}</span></td>` : `<td><span class="label label-success">${route.state}</span></td>`}
                        <td>$${route.collected}</td>
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
                    <td></td>
                    <td></td>
                </tr>`;
                $('#routesTable tbody').append($(row));
            }
        });
    }

    function productsDay(selectedDate) {
        $('#productsTable tbody').empty();
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
        $('#productsTable tbody').append($(loadingRow));
        $('#form-searchProductsByDate input[name="dateString"]').val(selectedDate);
        let form = $('#form-searchProductsByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#productsTable tbody').empty();
                response.data.forEach(product => {
                    let row = `<tr>
                        <td><span class="round"><i class="ti-shopping-cart"></i></span></td>
                        <td><h6>${product.name}</h6></td>
                        <td><h5>${product.dispatched}</h5></td>
                        <td><h5>${product.sold}</h5></td>
                        <td><h5>${product.returned}</h5></td>
                    </tr>`;
                    $('#productsTable tbody').append($(row));
                });
            },
            error: function (error) {
                $('#productsTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;
                $('#productsTable tbody').append($(row));
            }
        });
    }

    function expensesDay(selectedDate) {
        $('#expensesTable tbody').empty();
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
        $('#expensesTable tbody').append($(loadingRow));
        $('#form-searchExpensesByDate input[name="dateString"]').val(selectedDate);
        let form = $('#form-searchExpensesByDate');
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#expensesTable tbody').empty();
                response.data.forEach(expense => {
                    let row = `<tr>
                        <td><h6>${expense.dealer}</h6></td>
                        <td><h6>${expense.description}</h6></td>
                        <td><h5>$${expense.amount}</h5></td>
                    </tr>`;
                    $('#expensesTable tbody').append($(row));
                });
            },
            error: function (error) {
                $('#expensesTable tbody').empty();
                let row = `<tr>
                    <td>
                        <h6 class="text-danger">No se pudo cargar la información</h6>
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>`;
                $('#expensesTable tbody').append($(row));
            }
        });
    }
});

