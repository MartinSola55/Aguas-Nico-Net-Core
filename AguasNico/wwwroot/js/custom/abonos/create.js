$(document).ready(function () {
    $('#btnSubmit').click(function (e) {
        e.preventDefault();
        let products = [];
        let rows = $('#table_body tr');
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i];
            let quantity = parseInt(row.cells[1].children[0].value);
            if (quantity >= 0) {
                products.push({
                    Quantity: quantity,
                    Type: parseInt(row.dataset.id)
                });

            }
        }
        let productsData = {
            Abono: {
                Products: products
            }
        };

        let form = $("#form-submit");
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
                        window.location.href = window.location.origin + "/Abonos";
                    }
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
    });
});