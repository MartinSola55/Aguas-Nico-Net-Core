
$(document).ready(function () {
    $('#ExpensesDatePicker, #ProductsDatePicker, #RoutesDatePicker').bootstrapMaterialDatePicker({
        maxDate: new Date(),
        time: false,
        format: 'DD/MM/YYYY',
        cancelText: "Cancelar",
        weekStart: 1,
        lang: 'es',
    });

    $("#btnAddExpense").on("click", function () {
        $("#formContainer form input:not([type='hidden']").val("");
        $("#formContainer form select").val("");
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
    $('#BalanceDatePicker').on('change', function () {
        balanceDay($(this).val());
    });
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
            let totalRoutes = 0;

            response.routes.forEach(r => {
                const route = r.result;
                const row = `<tr class="clickable" data-url="/Routes/Details/${route.id}">
                        <td><h6>${route.dealer}</h6></td>
                        <td>${route.completedCarts}/${route.totalCarts}</td>
                        <td>
                            <ul class="mb-0 pl-0">
                                ${route.soldProducts.map(product => `<li>${product.name} (${product.sold})</li>`).join('')}
                            </ul>
                        </td>
                        <td>$${route.collected.toLocaleString("es-ES")}</td>
                    </tr>`;
                    totalRoutes += route.collected;
                $('#routesTable tbody').append($(row));
            });

            $('#routesTable tbody tr').on('click', function () {
                window.location.href = $(this).data('url');
            });

            $('#routesTotal').text(`$${totalRoutes.toLocaleString("es-ES")}`);
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
            let totalExpenses = 0;

            response.data.forEach(expense => {
                let row = `<tr>
                        <td><h6>${expense.dealer}</h6></td>
                        <td><h6>${expense.description}</h6></td>
                        <td><h5>$${expense.amount.toLocaleString("es-ES")}</h5></td>
                    </tr>`;
                $('#expensesTable tbody').append($(row));
                totalExpenses += expense.amount;
            });
            $('#expensesTotal').text(`$${totalExpenses.toLocaleString("es-ES")}`);
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
            $('#expensesTotal').text('No se pudo cargar la información');
        }
    });
}

function sendForm() {
    const form = document.getElementById("form-create-expense");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#modalCreate").modal('hide');
            Swal.fire({
                icon: 'success',
                title: response.message,
                confirmButtonColor: '#1e88e5',
            });
            expensesDay($('#ExpensesDatePicker').val());
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

function balanceDay(selectedDate)
{
    const form = document.getElementById("form-searchBalanceByDate");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            console.log(response);
        },
        error: function (error) {

        }
    });
}