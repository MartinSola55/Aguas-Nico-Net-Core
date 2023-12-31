function confirm() {
    let productosBajados = [];
    let rows_PB = $('#ProductosBajados tbody tr');
    for (let i = 0; i < rows_PB.length; i++) {
        let row = rows_PB[i];
        let quantity = parseInt(row.cells[2].children[0].value);
        if (quantity > 0) {
            productosBajados.push({
                Quantity: quantity,
                Type: parseInt(row.dataset.type)
            });
        }
    }
    
    let productosCargados = [];
    let rows_PD = $('#ProductosDevueltos tbody tr');
    for (let i = 0; i < rows_PD.length; i++) {
        let row = rows_PD[i];
        let quantity = parseInt(row.cells[2].children[0].value);
        if (quantity > 0) {
            productosCargados.push({
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
            PaymentMethods: methods,
            ReturnedProducts: productosDevueltos
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