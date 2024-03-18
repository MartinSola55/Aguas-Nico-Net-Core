$(document).ready(function () {
    $('#btnEditProducts').click(function () {
        if ($('#divSaveProducts').is(':visible')) {
            $('#divSaveProducts').hide();
            $('#TableProducts input[type="number"]').prop('disabled', true);
        } else {
            $('#divSaveProducts').show();
            $('#TableProducts input[type="number"]').prop('disabled', false);
        }
    });

    $('#btnSave').click(function () {
        // Verificar que no haya ningun stock menor a 0, en ese caso lanzar un sweet alert
        if ($('#TableProducts input[type="number"]').filter(function () { return $(this).val() < 0 }).length > 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Error',
                text: 'No puede haber stock negativo',
                confirmButtonColor: '#1e88e5',
            });
            return;
        }

        let products = [];
        let rows = $('#TableProducts tbody tr');
        for (let i = 0; i < rows.length; i++) {
            let row = rows[i];
            let stock = parseInt($(`input[data-product-id='${row.dataset.id}']`).val());
            if (stock >= 0) {
                products.push({
                    Stock: stock,
                    ProductID: parseInt(row.dataset.id),
                    ClientID: parseInt($('#Client_ID').val()),
                    Product: {
                        Type: parseInt(row.dataset.type)
                    }
                });

            }
        }
        let productsData = {
            products
        };

        let form = $("#form-updateProducts");
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize() + "&" + $.param(productsData),
            success: function (response) {
                $('#divSaveProducts').hide();
                $('#TableProducts input[type="number"]').prop('disabled', true);
                Swal.fire({
                    title: response.message,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#1e88e5',
                    confirmButtonText: 'OK',
                    allowOutsideClick: true,
                });
            },
            error: function (errorThrown) {
                Swal.fire({
                    icon: 'error',
                    title: errorThrown.responseJSON.title,
                    html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error != null ? errorThrown.responseJSON.error : "",
                    confirmButtonColor: '#1e88e5',
                });
            }
        });
    });

});
function detectEnter(event, btnID) {
    if (event.key === "Enter") {
        $(`#${btnID}`).click();
    }
}