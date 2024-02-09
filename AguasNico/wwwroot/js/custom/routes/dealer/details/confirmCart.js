function openModal(cartID, clientID) {
    $("#form-searchClientProducts input[name='id']").val(clientID);
    $("#form-confirmCart input[name='Cart.ID']").val(cartID);
    $("#form-confirmCart input[name='Cart.ClientID']").val(clientID);
    $("#form-confirmCart input:not([type='hidden']").val("");
    $("#divClientAbonos").hide();

    let form = $("#form-searchClientProducts");

    $('input[name="cartPaymentMethodOption"]').change(function () {
        cartPaymentMethod = $(this).val();
        if (cartPaymentMethod === "1") {
            $("#cartPaymentAmountContainer").show();
            $("#cartPaymentAmount").val("");
        } else {
            $("#cartPaymentAmountContainer").hide();
            $("#cartPaymentAmount").val("0");
        }
    });

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $("#clientProductsTable tbody").empty();
            $("#clientAbonoProductsTable tbody").empty();

            response.products.forEach(product => {
                $("#clientProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>$${product.price}</td>
                        <td><input type="number" class="form-control" name="quantity" value="" min="1"></td>
                    </tr>
                `);
            });
            if (response.abonoProducts.length > 0) {
                $("#divClientAbonos").show();
            }
            response.abonoProducts.forEach(product => {
                $("#clientAbonoProductsTable tbody").append(`
                    <tr data-type="${product.type}">
                        <td>${product.name}</td>
                        <td>${product.available}</td>
                        <td><input type="number" class="form-control" name="quantity" value="" min="1" max="${product.available}"></td>
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

function confirmCart(id) {
    const form = $(`#form-confirmCart_${id}`);
    // AGUS SEGUI ACA
    let abonoProducts = [];
    //const productRows = $(form).find('table[name="tableProducts"] tbody tr');
    //console.log(productRows);

    $("input[name='quantity']").on("input", function () {
        let total = 0;
        let rows = $(`table[data-name="tableProducts"][data-cart-id="${id}"] tbody tr`);
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i];
            if (row.cells[2].children[0].value == "") {
                continue;
            }
            let quantity = parseInt(row.cells[2].children[0].value);
            let price = parseInt(row.cells[1].innerText.replace("$", ""));
            total += quantity * price;
        }
        //$("#totalCart").text(`Total: $${total}`);
    });
    for (let i = 0; i < rows.length; i++) {
       let row = rows[i];
       let quantity = parseInt(row.cells[2].children[0].value);
       if (quantity <= 0) {
           Swal.fire({
               icon: 'warning',
               title: "Error",
               text: "La cantidad debe ser mayor a cero",
               confirmButtonColor: '#1e88e5',
           });
           return false;
       }
       if (quantity > parseInt(parseInt(row.cells[1].textContent.trim()))) {
           Swal.fire({
               icon: 'warning',
               title: "Error",
               text: "No se puede bajar m�s productos del abono de los que dispone",
               confirmButtonColor: '#1e88e5',
           });
           return false;
       }
       if (quantity > 0) {
           abonoProducts.push({
               Quantity: quantity,
               Type: parseInt(row.dataset.type)
           });

       }
    }

    let products = [];
    rows = $(`table[data-name="tableProducts"][data-cart-id="${id}"] tbody tr`);//$('#clientProductsTable tbody tr');
    for (let i = 0; i < rows.length; i++) {
       let row = rows[i];
       let quantity = parseInt(row.cells[2].children[0].value);
       if (quantity <= 0) {
           Swal.fire({
               icon: 'warning',
               title: "Error",
               text: "La cantidad debe ser mayor a cero",
               confirmButtonColor: '#1e88e5',
           });
           return false;
       }
       if (quantity > 0) {
           products.push({
               Quantity: quantity,
               Type: parseInt(row.dataset.type)
           });

       }
    }

    //let methods = [];
    //if ($("#cartPaymentMethod").val() != "" && $("#cartPaymentAmount").val() != "") {
    //    methods.push({
    //        PaymentMethodID: $('input[name="cartPaymentMethodOption"]:checked').val(),
    //        Amount: $("#cartPaymentAmount").val()
    //    });
    //}

    let productsData = {
       Cart: {
           Products: products,
           PaymentMethods: methods,
           AbonoProducts: abonoProducts
       }
    };
    if (products.length <= 0 & abonoProducts.length <=0 && methods[0].Amount <= 0) {
       Swal.fire({
           icon: 'warning',
           title: "Error",
           text: "No se puede confirmar una bajada sin productos y dinero.",
           confirmButtonColor: '#1e88e5',
       });
       return false;
    }

    //let form = $("#form-confirmCart");
    //$.ajax({
    //    url: $(form).attr('action'),
    //    method: $(form).attr('method'),
    //    data: $(form).serialize() + "&" + $.param(productsData),
    //    success: function (response) {
    //        Swal.fire({
    //            title: response.message,
    //            icon: 'success',
    //            showCancelButton: false,
    //            confirmButtonColor: '#1e88e5',
    //            confirmButtonText: 'OK',
    //            allowOutsideClick: false,
    //        }).then((result) => {
    //            if (result.isConfirmed) {
    //                window.location.reload();
    //            }
    //        });
    //    },
    //    error: function (errorThrown) {
    //        Swal.fire({
    //            icon: 'error',
    //            title: errorThrown.responseJSON.title,
    //            html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error,
    //            confirmButtonColor: '#1e88e5',
    //        });
    //    }
    //});
}

$(document).ready(function () {
    $("#cartPaymentMethod").on("change", function () {
        $("#cartPaymentAmountContainer").show();
    });

    // Evento para detectar cambios en los métodos de pago y manejarlos por formulario
    $('input[name="cartPaymentMethodOption"]').change(function () {
        var cartId = $(this).closest('form').attr('id').split('_')[1];
        var cartPaymentMethod = $(this).val();
        manejarMetodoPago(cartId, cartPaymentMethod);
    });

    // Función para manejar los métodos de pago por formulario
    function manejarMetodoPago(cartId, cartPaymentMethod) {
        if (cartPaymentMethod === "1") {
            $("#cartPaymentAmountContainer_" + cartId).show();
            $("#cartPaymentAmount_" + cartId).val("");
        } else {
            $("#cartPaymentAmountContainer_" + cartId).hide();
            $("#cartPaymentAmount_" + cartId).val("0");
        }
    }

    // Evento para detectar cambios en las inputs y actualizar el total específico
    $('input[data-cart-input]').on('input', function () {
        var cartId = $(this).closest('form').attr('id').split('_')[1];
        calcularTotal(cartId);
    });

    // Función para calcular y actualizar el total específico
    function calcularTotal(cartId) {
        var total = 0;

        // Iterar sobre cada fila de la tabla específica
        $('table[data-name="tableProducts"][data-cart-id="' + cartId + '"] tbody tr').each(function () {
            var price = parseFloat($(this).find('.price').text().replace('$', '').replace('.', '').trim());
            $(this).find('.price').text().replace(',', '').trim();
            var cantidad = parseInt($(this).find('input[data-cart-input]').val()) || 0;

            total += price * cantidad;
        });

        // Actualizar el texto del totalCart específico
        $('#totalCartContainer_' + cartId + ' .totalCart').text('Total: $' + total.toFixed(2));
    }
});