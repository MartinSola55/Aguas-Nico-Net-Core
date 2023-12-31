$(document).ready(function () {
    $('#DataTable').DataTable({
        "order": false,
        "paging": false,
        "info": false,
        "searching": false,
    });

    $('#Client_HasInvoice').change(function () {
        if ($(this).is(':checked')) {
            $('#invoiceDataContainer').show();
            $('#Client_InvoiceType').val('');
            $('#Client_TaxCondition').val('');
            $('#Client_CUIT').val('');
        } else {
            $('#invoiceDataContainer').hide();
        }
    });

    $('#btnSubmit').click(function (e) {
        e.preventDefault();
        let products = [];
        let rows = $('#table_body tr');
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i];
            let stock = parseInt(row.cells[2].children[0].value);
            if (stock >= 0) {
                products.push({
                    Stock: stock,
                    ProductID: parseInt(row.dataset.id)
                });

            }
        }
        let productsData = {
            Client: {
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
                        window.location.href = window.location.origin;
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