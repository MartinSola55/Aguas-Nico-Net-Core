function getReturnedProducts(clientID, clientName, cartID) {
    $("#returnModalTitle").text("Devolución de productos de " + clientName);

    let form = $("#form-getReturnedProducts");
    let formConfirm = $("#form-returnProducts");
    form.find("input[name='clientID']").val(clientID);
    form.find("input[name='cartID']").val(cartID);
    formConfirm.find("input[name='clientID']").val(clientID);
    formConfirm.find("input[name='cartID']").val(cartID);
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#returnedProductsTable tbody").empty();
            $("#modalReturned").modal("show");
            response.data.forEach(product => {
                $("#returnedProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>
                            <input type="number" class="form-control" name="returnedProduct" min="0" value="${product.quantity}">
                        </td>
                    </tr>
                `);
            });

            $("#returnedProductsTable input[name='returnedProduct']").on("change", function () {
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

function confirmReturned() {
    let products = [];
    let rows = $('#returnedProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[1].children[0].value);
        if (quantity > 0) {
            products.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type),
            });
        }
    }

    let productsData = {
        products: products
    };

    let form = $("#form-returnProducts");
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