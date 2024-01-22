function confirm() {
    let productosAbono = [];
    let rows = $('#ProductosAbono tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[1].children[0].value);
        if (quantity > 0) {
            productosAbono.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });
        }
    }

    let productosBajados = [];
    rows = $('#ProductosBajados tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[1].children[0].value);
        if (quantity > 0) {
            productosBajados.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });
        }
    }
    
    let productosDevueltos = [];
    rows = $('#ProductosDevueltos tbody tr');
    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let quantity = parseInt(row.cells[1].children[0].value);
        if (quantity > 0) {
            productosDevueltos.push({
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
            Products: productosBajados,
            AbonoProducts: productosAbono,
            PaymentMethods: methods,
            ReturnedProducts: productosDevueltos
        }
    };
    if (productosBajados.length <= 0 && productosAbono.length <= 0 && methods[0].Amount <= 0) {
        Swal.fire({
            icon: 'warning',
            title: "Error",
            text: "No se puede confirmar una bajada sin productos y dinero.",
            confirmButtonColor: '#1e88e5',
        });
        return false;
    }

    let form = $("#form-edit");
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
                    window.location.href = window.location.origin + "/Routes/Details/" + response.data;
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