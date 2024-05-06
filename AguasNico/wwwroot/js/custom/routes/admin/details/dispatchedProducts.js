function getDispatchedProducts(routeID) {
    let form = $("#form-dispatchedProducts");
    let formConfirm = $("#form-confirmDispatchedProducts");
    form.find("input[name='routeID']").val(routeID);
    formConfirm.find("input[name='routeID']").val(routeID);
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#dispatchedProductsTable tbody").empty();
            $("#modalDispatched").modal("show");
            response.data.forEach(product => {
                $("#dispatchedProductsTable tbody").append(`
                    <tr data-id="${product.type}">
                        <td>${product.name}</td>
                        <td>
                            <input type="number" class="form-control" name="dispatchedProduct" min="0" value="${product.quantity}">
                        </td>
                    </tr>
                `);
            });

            $("#dispatchedProductsTable input[name='dispatchedProduct']").on("change", function () {
                let quantity = parseInt($(this).val());
                let min = parseInt($(this).attr("min"));
                if (quantity < min) {
                    $(this).val(min);
                }
            });
        },
        error: function (errorThrown) {
            Swal.fire({
                icon: 'error',
                title: errorThrown.responseJSON.message,
                confirmButtonColor: '#1e88e5',
            });
        }
    });
}

function confirmDispatched() {
    let products = [];
    let rows = $('#dispatchedProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[1].children[0].value);
        if (quantity > 0) {
            products.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.id),
            });
        }
    }

    let productsData = {
        products: products
    };

    let form = $("#form-confirmDispatchedProducts");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize() + "&" + $.param(productsData),
        success: function (response) {
            Swal.fire({
                title: response.message,
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#1e88e5',
                confirmButtonText: 'OK',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
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

function setDispenserPrice() {
    Swal.fire({
        title: "Ingrese el precio del dispenser",
        input: 'number',
        inputAttributes: {
            min: 0,
            step: 1,
        },
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-success waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        },
        inputValidator: (value) => {
            if (!value || value < 0) {
                return 'Por favor, ingrese un monto válido';
            }
        },
    }).then((result) => {
        if (result.isConfirmed) {
            const form = $("#form-setDispenserPrice");
            form.find("input[name='price']").val(result.value);
            $.ajax({
                url: $(form).attr('action'),
                method: $(form).attr('method'),
                data: $(form).serialize(),
                success: function (response) {
                    Swal.fire({
                        title: response.message,
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#1e88e5',
                        confirmButtonText: 'OK',
                        allowOutsideClick: false,
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
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
    });
}