function confirmCart(id) {
    const form = $(`#form-confirmCart_${id}`);
    let abonoProducts = [];

    const abonoProductRows = $(`table[data-name="tableAbonoProducts"][data-cart-id="${id}"] tbody tr`);

    for (let i = 0; i < abonoProductRows.length; i++) {
       let row = abonoProductRows[i];
       let quantity = parseInt(row.cells[1].children[0].value);
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
    const productRows = $(`table[data-name="tableProducts"][data-cart-id="${id}"] tbody tr`);
    for (let i = 0; i < productRows.length; i++) {
       let row = productRows[i];
       let quantity = parseInt(row.cells[1].children[0].value);
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

    let methods = [];
    const amount = $(`#cartPaymentAmount_${id}`).val();
    const method = $(`input[id="cartPaymentMethod_${id}"]:checked`).val();
    if (amount) {
        methods.push({
            PaymentMethodID: parseInt(method),
            Amount: parseFloat(amount)
        });
    }

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
    $('input[name="quantity"]').on('input', function () {
        const cartId = $(this).closest('form').attr('id').split('_')[1];
        calcularTotal(cartId);
    });

    // Función para calcular y actualizar el total específico
    function calcularTotal(cartId) {
        let total = 0;

        // Iterar sobre cada fila de la tabla específica
        $('table[data-name="tableProducts"][data-cart-id="' + cartId + '"] tbody tr').each(function () {
            const price = parseFloat($(this).find('.price').text().replace('$', '').replace('.', '').trim());
            $(this).find('.price').text().replace(',', '').trim();
            const cantidad = parseInt($(this).find('input[name="quantity"]').val()) || 0;

            total += price * cantidad;
        });

        // Actualizar el texto del totalCart específico
        $('#totalCartContainer_' + cartId + ' .totalCart').text('Total: $' + total);
        
        // Verificar el método de pago seleccionado
        const cartPaymentMethod = $('input[name="cartPaymentMethodOption"]:checked').val();

        // Si el método de pago es "1", actualizar el valor de la input #cartPaymentAmount_
        if (cartPaymentMethod === "1") {
            $("#cartPaymentAmount_" + cartId).val(total);
        }
    }
});