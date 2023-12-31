function openModal(cartID, clientID) {
    $("#form-searchClientProducts input[name='id']").val(clientID);
    $("#form-confirmCart input[name='Cart.ID']").val(cartID);
    $("#form-confirmCart input[name='Cart.ClientID']").val(clientID);
    $("#form-confirmCart input:not([type='hidden']").val("");
    $("#cartPaymentMethod").val("");
    $("#cartPaymentAmountContainer").hide();

    let form = $("#form-searchClientProducts");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#clientProductsTable tbody").empty();
            response.data.forEach(product => {
                $("#clientProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>$${product.price}</td>
                        <td><input type="number" class="form-control" name="quantity" value="" min="1"></td>
                    </tr>
                `);
            });
            $("#modalConfirmation").modal('show');
            $("input[name='quantity']").on("input", function () {
                let total = 0;
                let rows = $('#clientProductsTable tbody tr');
                for (let i = 0; i < rows.length; i++) {
                    let row = rows[i];
                    if (row.cells[2].children[0].value == "") {
                        continue;
                    }
                    let quantity = parseInt(row.cells[2].children[0].value);
                    let price = parseInt(row.cells[1].innerText.replace("$", ""));
                    total += quantity * price;
                }
                $("#totalCart").text(`Total: $${total}`);
            });
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

function confirmCart() {
    let products = [];
    let rows = $('#clientProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[2].children[0].value);
        if (quantity > 0) {
            products.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });

        }
    }

    let methods = [];
    methods.push({
        PaymentMethodID: $("#cartPaymentMethod").val(),
        Amount: $("#cartPaymentAmount").val()
    });

    let productsData = {
        Cart: {
            Products: products,
            PaymentMethods: methods,
        }
    };
    if (products.length <= 0 && methods[0].Amount <= 0) {
        Swal.fire({
            icon: 'warning',
            title: "Error",
            text: "No se puede confirmar una bajada sin productos y dinero.",
            confirmButtonColor: '#1e88e5',
        });
        return false;
    }

    let form = $("#form-confirmCart");
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
                    window.location.reload();
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

$(document).ready(function () {
    $("#cartPaymentMethod").on("change", function () {
        $("#cartPaymentAmountContainer").show();
    });
});