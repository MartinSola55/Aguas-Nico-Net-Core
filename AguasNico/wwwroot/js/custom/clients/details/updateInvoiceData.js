$(document).ready(function () {
    $('#btnEditInvoice').click(function () {
        if ($('#divSaveInvoiceData').is(':visible')) {
            $('#divSaveInvoiceData').hide();
            $('#form-invoice input[type="text"]').prop('disabled', true);
            $('#form-invoice select').prop('disabled', true);
        } else {
            $('#divSaveInvoiceData').show();
            $('#form-invoice input[type="text"]').prop('disabled', false);
            $('#form-invoice select').prop('disabled', false);
        }
    });

    $('#btnSaveInvoiceData').click(function () {
        let form = $("#form-invoice");
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function (response) {
                $('#divSaveInvoiceData').hide();
                $('#form-invoice input[type="text"]').prop('disabled', true);
                $('#form-invoice select').prop('disabled', true);
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
                    text: errorThrown.responseJSON.message,
                    confirmButtonColor: '#1e88e5',
                });
            }
        });
    });
});